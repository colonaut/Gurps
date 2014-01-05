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


            var attempt1DebugDictionary = UrlInfoBase.Find(typeof(GurpsCharacter));

            var attempt2DebugDictionary = RouteInfo.FindRouteInfos(typeof(GurpsCharacter));

            var attempt3DebugDictionary = RouteInfo.FindRouteInfos(typeof(GameSession));



            var attempt4DebugDictionary = UrlInfoBase.Find(typeof(Article)) as IEnumerable<RouteInfo>;
            var attempt5DebugDictionary = UrlInfoBase.Find(typeof(Category)) as IEnumerable<RouteInfo>;
            
            //CollectionJson
            routes.MapRoute(
                name: "CollectionJson",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }

    }
}