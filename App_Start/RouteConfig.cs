using System.Web.Mvc;
using System.Web.Routing;
using CollectionJsonExtended.Client;
using CollectionJsonExtended.Core;
using MedienKultur.Gurps.Controllers;
using MedienKultur.Gurps.Models;
using MedienKultur.RequireScriptResolver;

namespace MedienKultur.Gurps
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //CharactersController.RegisterRoutes(routes);

            //GameSessionsController.RegisterRoutes(routes);

            ArticlesController.RegisterRoutes(routes);

            routes.MapMvcAttributeRoutes(); //also maps the CollectionJsonRoutes

            // just a debug ****
            
            var attempt1DebugDictionary = SingletonFactory<UrlInfoCollection>.Instance
                .Find(typeof(GurpsCharacter));

            var attempt3DebugDictionary = SingletonFactory<UrlInfoCollection>.Instance
                .Find<RouteInfo>(typeof(GameSession));


            //********

            routes.MapRoute(
                name: "CollectionJson",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }

    }
}