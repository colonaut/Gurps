using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MedienKultur.CollectionJsonExtended;
using MedienKultur.Gurps.Models;
using Raven.Client;

namespace MedienKultur.Gurps.Controllers
{
    public class GameSessionsController : Controller
    {
        const string BaseUri = "api/gamesessions";
        readonly IDocumentSession _ravenSession;

        public GameSessionsController(IDocumentSession ravenSession)
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
                new {controller = "GameSessions", action = "DeleteGameSession"},
                new {httpMethod = new HttpMethodConstraint("DELETE")}
            );

            routes.MapRoute(
                BaseUri + "/{id} PUT",
                BaseUri + "/{id}",
                new { controller = "GameSessions", action = "UpdateGameSession" },
                new {httpMethod = new HttpMethodConstraint("PUT")}
            );

            routes.MapRoute(
                BaseUri + "/{id} GET",
                BaseUri + "/{id}",
                new { controller = "GameSessions", action = "Get" },
                new {httpMethod = new HttpMethodConstraint("GET")}
            );

            routes.MapRoute(
                BaseUri + " GET",
                BaseUri + "",
                new { controller = "GameSessions", action = "Query" },
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
                new { controller = "GameSessions", action = "CreateGameSession" },
                new {httpMethod = new HttpMethodConstraint("POST")}
            );
        }
        #endregion

        public CollectionJsonResult<GameSession> Query()
        {
            var models = _ravenSession.Query<GameSession>();
            return new CollectionJsonResult<GameSession>(models);
        }

        public CollectionJsonResult<GameSession> Get(int id)
        {
            var model = _ravenSession.Load<GameSession>(id);
            if (model == null)
            {
                Response.StatusCode = 404;
                Response.StatusDescription = "GameSession was not found";
            }

            return new CollectionJsonResult<GameSession>(model);
        }

        //POST
        public HttpResponseMessage CreateGameSession(CollectionJsonReader<GameSession> reader)
        {
            var entity = new GameSession();
            _ravenSession.Store(entity);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }



        public ActionResult Index()
        {
            var sessions = _ravenSession.Query<GameSession>()
                .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                .AsEnumerable();

            return View(sessions);
        }

    }
}
