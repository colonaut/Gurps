using System.Web.Mvc;
using System.Web.Routing;
using CollectionJsonExtended.Client;
using CollectionJsonExtended.Client.Attributes;
using MedienKultur.Gurps.Controllers;
using MedienKultur.RequireScriptResolver;

namespace MedienKultur.Gurps.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RequireScriptController.RegisterRoutes(routes);

            //CharactersController.RegisterRoutes(routes);

            //GameSessionsController.RegisterRoutes(routes);

            ArticlesController.RegisterRoutes(routes);

            routes.MapMvcAttributeRoutes(); //also maps the CollectionJsonRoutes

            var oldDebugDictionary = RouteCollectionJsonAttribute._debugInstancesDictionary;

            var newDebugDictionary = RouteInfo.InternalCache;
            
            //CollectionJson
            routes.MapRoute(
                name: "CollectionJson",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }

    }
}