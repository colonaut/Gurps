using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using CollectionJsonExtended.Client;
using CollectionJsonExtended.Client.Attributes;
using CollectionJsonExtended.Core;
using MedienKultur.Gurps.Controllers;
using MedienKultur.Gurps.Models;
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


            var attempt1DebugDictionary = Singleton<UrlInfoCollection>.Instance.Find(typeof(GurpsCharacter));

            var attempt2DebugDictionary = Singleton<UrlInfoCollection>.Instance.Find<RouteInfo>(typeof(GurpsCharacter));

            var attempt3DebugDictionary = Singleton<UrlInfoCollection>.Instance.Find<RouteInfo>(typeof(GameSession));


            
            //CollectionJson
            routes.MapRoute(
                name: "CollectionJson",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }

    }
}