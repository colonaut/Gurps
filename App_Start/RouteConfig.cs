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
using CollectionJsonExtended.Client.Extensions;
using CollectionJsonExtended.Core;
using MedienKultur.Gurps.Controllers;
using MedienKultur.Gurps.Models;
using MedienKultur.RequireScriptResolver;
using Raven.Client.Linq.Indexing;
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

            routes.PublishCollectionJsonAttributeRoutes();

            //CollectionJson
            routes.MapRoute(
                name: "CollectionJson",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }

    }
}