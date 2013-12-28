using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Routing;
using System.Web.Routing;
using CollectionJsonExtended.Client;
using CollectionJsonExtended.Client.Attributes;
using CollectionJsonExtended.Client.Extensions;
using CollectionJsonExtended.Core;
using MedienKultur.Gurps.Controllers;
using MedienKultur.Gurps.Models;
using MedienKultur.RequireScriptResolver;
using Raven.Client.Linq.Indexing;
using Raven.Database.Extensions;
using Raven.Database.Linq.PrivateExtensions;
using StructureMap.TypeRules;

namespace MedienKultur.Gurps.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RequireScriptController.RegisterRoutes(routes);

            CharactersController.RegisterRoutes(routes);

            GameSessionsController.RegisterRoutes(routes);

            ArticlesController.RegisterRoutes(routes);

            routes.MapMvcAttributeRoutes(); //also maps the CollectionJsonRoutes

            //TestMe.ScanMethods(typeof(GameSessionsController));
            TestMe.ScanControllers();
            
            //CollectionJson
            routes.MapRoute(
                name: "CollectionJson",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }

    }


    



    //what of this is needed?

    internal class RoutesInfo
    {
        public RoutesInfo(Type entityType)
        {
            EntityType = entityType;
            //TODO: throws if not found. We will have to set a better error message via try catch
            //an entity must have an id. otherwise the cj spec is useless.
            //we might think of open the code at some place to treat another property as id.
            IdentifierType = entityType.GetProperty("Id",
                BindingFlags.IgnoreCase
                | BindingFlags.Instance
                | BindingFlags.Public)
                .PropertyType;
            
            ItemLinks = new List<RouteInfo>();
            Links = new List<RouteInfo>();
            Queries = new List<RouteInfo>();
        }

        public Type EntityType { get; private set; }
        public Type IdentifierType { get; private set; }

        public RouteInfo Create { get; set; }
        public RouteInfo Delete { get; set; }
        public RouteInfo Item { get; set; }
        public IList<RouteInfo> ItemLinks { get; private set; }

        public IList<RouteInfo> Links { get; private set; }
        public IList<RouteInfo> Queries { get; private set; }
    };

    internal class RouteInfo
    {
        public string BaseUriTemplate { get; set; }
        public string RelativeUriTemplate { get; set; } //TODO (?) validate the route on consistence with type of entity (Id, Guid, etc.) and make parsable for output
        public RenderType Render { get; set; }
        public string Rel { get; set; }
    }

    public class TestMe
    {
        static readonly IList<RoutesInfo> RoutesInfoCollection;
        
        static TestMe()
        {
            RoutesInfoCollection = new List<RoutesInfo>();
        }
        
        
        internal static RoutesInfo GetRoutesInfo(Type entityType)
        {
            return RoutesInfoCollection.SingleOrDefault(x => x.EntityType == entityType);
        }

        public static void ScanControllers()
        {
            Assembly.GetCallingAssembly().GetTypes()
                //Find our controller types    
                .Where(type => type.IsSubclassOf(typeof (Controller)))
                //Do something with result
                .Apply(ScanMethods);
        }

        public static void ScanMethods(Type controllerType)
        {
            //var controllerDescriptor = new ReflectedControllerDescriptor(controllerType);

            var routePrefixTemplate =
                (string)controllerType.GetCustomAttributes(typeof(RoutePrefixAttribute))
                    .Select(a => a.GetType().GetRuntimeProperty("Prefix").GetValue(a))
                    .SingleOrDefault()
                ?? string.Empty;

            controllerType.GetMethods(BindingFlags.Public
                                      | BindingFlags.Instance
                                      | BindingFlags.DeclaredOnly)
                //Find our methods
                .Where(methodInfo =>
                    methodInfo.ReturnType.IsGenericType
                    && methodInfo.ReturnType.IsSubclassOf(typeof (CollectionJsonResult)))
                //Do something with result
                .Apply(methodInfo =>
                    AddRoutesInfoFromAttributes(methodInfo, routePrefixTemplate));

            var x = RoutesInfoCollection;
            var debugMyResult = x;

        }


        static void AddRoutesInfoFromAttributes(MethodInfo methodInfo, string routePrefixTemplate)
        {
            
            //TODO versuche sction conroller area schon zu parsen (new Uri?) sollte aber eher im ececute gemacht werden
            var entityType = methodInfo.ReturnType.GetGenericArguments().Single();

            var routesInfo = TestMe.GetRoutesInfo(entityType)
                              ?? CreateRoutesInfo(entityType);

            var createAttribute =
                methodInfo.GetCustomAttributes<RouteCollectionJsonCreateAttribute>().SingleOrDefault();
            if (createAttribute != null)
                routesInfo.Create = new RouteInfo
                {
                    BaseUriTemplate = routePrefixTemplate,
                    RelativeUriTemplate = createAttribute.Template,
                    Render = createAttribute.Render
                };

            var deleteAttribute =
                methodInfo.GetCustomAttributes<RouteCollectionJsonDeleteAttribute>()
                    .SingleOrDefault();
            if (deleteAttribute != null)
                routesInfo.Delete = new RouteInfo
                {
                    BaseUriTemplate = routePrefixTemplate,
                    RelativeUriTemplate = deleteAttribute.Template,
                    Render = deleteAttribute.Render
                };

            var queryAttribute =
                methodInfo.GetCustomAttributes<RouteCollectionJsonQueryAttribute>()
                    .SingleOrDefault();
            if (queryAttribute != null)
                routesInfo.Queries.Add(new RouteInfo
                {
                    BaseUriTemplate = routePrefixTemplate,
                    RelativeUriTemplate = queryAttribute.Template,
                    Rel = queryAttribute.Rel,
                    Render = queryAttribute.Render
                });

            var itemAttributes =
                methodInfo.GetCustomAttributes<RouteCollectionJsonItemAttribute>()
                    .ToList();
            var itemAttribute =
                itemAttributes.SingleOrDefault(a => a.Rel == null);
            if (itemAttribute != null)
                routesInfo.Item = new RouteInfo
                {
                    BaseUriTemplate = routePrefixTemplate,
                    RelativeUriTemplate = itemAttribute.Template
                };
            var itemLinkAttribute =
                itemAttributes.SingleOrDefault(a => a.Rel != null);
            if (itemLinkAttribute != null)
                routesInfo.ItemLinks.Add(new RouteInfo
                {
                    BaseUriTemplate = routePrefixTemplate,
                    RelativeUriTemplate = itemLinkAttribute.Template,
                    Rel = itemLinkAttribute.Rel,
                    Render = itemLinkAttribute.Render
                });
        }

        static RoutesInfo CreateRoutesInfo(Type entityType)
        {
            var routingInfo = new RoutesInfo(entityType);
            RoutesInfoCollection.Add(routingInfo);
            return routingInfo;
        }
    }
}