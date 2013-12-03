using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Routing;
using MedienKultur.CollectionJsonExtended;
using MedienKultur.Gurps.Models;
using Raven.Client;

namespace MedienKultur.Gurps.Controllers
{
    
    public class CharactersController : Controller
    {
        const string BaseUri = "api/characters";
        readonly IDocumentSession _ravenSession;

        public CharactersController(IDocumentSession ravenSession)
        {
            _ravenSession = ravenSession;
        }

        #region RegisterRoutes()
        internal static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                BaseUri + "/{id} DELETE",
                BaseUri + "/{id}",
                new {controller = "Characters", action = "DeleteCharacter"},
                new {httpMethod = new HttpMethodConstraint("DELETE")}
            );

            routes.MapRoute(
                BaseUri + "/{id} PUT",
                BaseUri + "/{id}",
                new {controller = "Characters", action = "UpdateCharacter"},
                new {httpMethod = new HttpMethodConstraint("PUT")}
            );

            routes.MapRoute(
                BaseUri + "/{id} GET",
                BaseUri + "/{id}",
                new {controller = "Characters", action = "Get"},
                new {httpMethod = new HttpMethodConstraint("GET")}
            );

            routes.MapRoute(
                BaseUri + " GET",
                BaseUri + "",
                new {controller = "Characters", action = "Query"},
                new {httpMethod = new HttpMethodConstraint("GET")}
                );

            //routes.MapRoute(
            //    BaseUri + "/{id} POST",
            //    BaseUri + "/{id}",
            //    new {controller = "Characters", action = "AddCharacter"},
            //    new {httpMethod = new HttpMethodConstraint("POST")}
            //);

            routes.MapRoute(
                BaseUri + " POST",
                BaseUri,
                new {controller = "Characters", action = "AddCharacter"},
                new {httpMethod = new HttpMethodConstraint("POST")}
            );
        }
        #endregion


        public CollectionJsonResult<GurpsCharacter> Query()
        {
            var models = _ravenSession.Query<GurpsCharacter>();
            return new CollectionJsonResult<GurpsCharacter>(models);
        }

        public CollectionJsonResult<GurpsCharacter> Get(int id)
        {
            var model = _ravenSession.Load<GurpsCharacter>(id);
            if (model == null)
            {
                Response.StatusCode = 404;
                Response.StatusDescription = "Character was not found";
            }

            return new CollectionJsonResult<GurpsCharacter>(model);
        }

        //POST
        public HttpResponseMessage AddCharacter(CollectionJsonReader<GurpsCharacter> reader)
        {
            var entity = reader.Entity;
            if (entity == null)
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            
            _ravenSession.Store(entity);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }



        public ActionResult Index()
        {
            var characters = _ravenSession.Query<GurpsCharacter>()
                .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                .AsEnumerable();

            return View(characters);
        }

        //
    }
}
