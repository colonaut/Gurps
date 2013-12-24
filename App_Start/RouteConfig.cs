using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
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

            TestMe.ScanMethods(typeof(GameSessionsController));
            
            //Default
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }

    }


    



    //what of this is needed?
    public class TestMe
    {
        
        public static void ScanControllers()
        {

            var controllerTypes =
                Assembly.GetCallingAssembly().GetTypes().Where(type => type.IsSubclassOf(typeof (Controller))).ToList();

            foreach (var controllerType in controllerTypes)
            {
                ScanMethods(controllerType);

            }


            //var controlList = controllers.Select(controller =>
            //    new
            //    {
            //        Actions = GetActions(controller),
            //        Name = controller.Name,
            //    }).ToList();
        }

        public static void ScanMethods(Type controllerType)
        {
            var controllerDescriptor = new ReflectedControllerDescriptor(controllerType);
            var collectionJsonMethods =  controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(mi =>
                    mi.ReturnType.IsGenericType
                    && mi.ReturnType.IsSubclassOf(typeof(CollectionJsonResult))
                ).ToList();

        }

        public static List<String> GetActions(Type controller)
        {
            // List of links
            var items = new List<String>();

            // Get a descriptor of this controller
            var controllerDesc = new ReflectedControllerDescriptor(controller);

            // Look at each action in the controller
            foreach (var action in controllerDesc.GetCanonicalActions())
            {
                // Get any attributes (filters) on the action
                var attributes = action.GetCustomAttributes(false);

                // Look at each attribute
                var validAction =
                    attributes.All(filter => !(filter is HttpPostAttribute) && !(filter is ChildActionOnlyAttribute));

                // Add the action to the list if it's "valid"
                if (validAction)
                    items.Add(action.ActionName);
            }
            return items;
        }

    }
}