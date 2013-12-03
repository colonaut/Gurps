using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Routing;
using MedienKultur.Gurps.Models;
using Raven.Client;

namespace MedienKultur.Gurps.Controllers
{
    public class ArticlesController : Controller
    {
        const string BaseUri = "api/articles";
        readonly IDocumentSession _ravenSession;

        public ArticlesController(IDocumentSession ravenSession)
        {
            _ravenSession = ravenSession;
        }


        #region RegisterRoutes()
        /// <summary>
        /// Registers the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        internal static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                BaseUri + "/{id} DELETE",
                BaseUri + "/{id}",
                new { controller = "Articles", action = "DeleteArticle" },
                new { httpMethod = new HttpMethodConstraint("DELETE") }
            );

            routes.MapRoute(
                BaseUri + "/{id} PUT",
                BaseUri + "/{id}",
                new { controller = "Articles", action = "UpdateArticle" },
                new { httpMethod = new HttpMethodConstraint("PUT") }
            );

            routes.MapRoute(
                BaseUri + "/{id} GET",
                BaseUri + "/{id}",
                new { controller = "Articles", action = "Get" },
                new { httpMethod = new HttpMethodConstraint("GET") }
            );

            routes.MapRoute(
                BaseUri + " GET",
                BaseUri + "",
                new { controller = "Articles", action = "Query" },
                new { httpMethod = new HttpMethodConstraint("GET") }
                );

            //routes.MapRoute(
            //    BaseUri + "/{id} POST",
            //    BaseUri + "/{id}",
            //    new {controller = "Articles", action = "AddArticle"},
            //    new {httpMethod = new HttpMethodConstraint("POST")}
            //);

            routes.MapRoute(
                BaseUri + " POST",
                BaseUri,
                new { controller = "Articles", action = "AddCharacter" },
                new { httpMethod = new HttpMethodConstraint("POST") }
            );
        }
        #endregion


        // GET: /Article/
        public ActionResult Index()
        {
            var articles = _ravenSession.Query<Article>()
                .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                .AsEnumerable();
            return View(articles);
        }

    }
}
