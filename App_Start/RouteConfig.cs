using System.Web.Mvc;
using System.Web.Routing;
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

            CharactersController.RegisterRoutes(routes);

            GameSessionsController.RegisterRoutes(routes);

            ArticlesController.RegisterRoutes(routes);

            //Default
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}