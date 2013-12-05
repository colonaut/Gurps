using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MedienKultur.CollectionJsonExtended;
using MedienKultur.Gurps.Models;
using Raven.Client;
using Raven.Database.Extensions;

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
                new {controller = "GameSessions", action = "Delete"},
                new {httpMethod = new HttpMethodConstraint("DELETE")}
            );

            routes.MapRoute(
                BaseUri + "/{id} PUT",
                BaseUri + "/{id}",
                new { controller = "GameSessions", action = "Update" },
                new {httpMethod = new HttpMethodConstraint("PUT")}
            );

            routes.MapRoute(
                BaseUri + "/{id} GET",
                BaseUri + "/{id}",
                new { controller = "GameSessions", action = "Get" },
                new {httpMethod = new HttpMethodConstraint("GET")}
            );

            routes.MapRoute(
                BaseUri + " QUERY",
                BaseUri + "",
                new { controller = "GameSessions", action = "Query" },
                new { httpMethod = new HttpMethodConstraint("QUERY") }
            );

            routes.MapRoute(
                BaseUri + " POST",
                BaseUri,
                new { controller = "GameSessions", action = "Create" },
                new {httpMethod = new HttpMethodConstraint("POST")}
            );
        }
        #endregion

        //QUERY
        public CollectionJsonResponse<GameSession> Query()
        {
            var models = _ravenSession.Query<GameSession>()
                .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite());
                //.AsEnumerable();


            return new CollectionJsonResponse<GameSession>(models);
        }

        //GET
        public CollectionJsonResponse<GameSession> Get(int id)
        {
            var model = _ravenSession.Load<GameSession>(id);
            //if (model == null)
            //{
            //    Response.StatusCode = 404; //therfore we need an error response (wrapped in response!)
            //    Response.StatusDescription = "GameSession was not found";
            //} now done from result...?
            
            return new CollectionJsonResponse<GameSession>(model);
        }

        //POST
        public CollectionJsonResponse Create(CollectionJsonReader<GameSession> reader)
        {
            var entity = new GameSession();
            _ravenSession.Store(entity);

            return new CollectionJsonResponse<GameSession>(entity);
        }

        //DELETE
        public CollectionJsonResponse Delete(int id)
        {
            var entity = _ravenSession.Load<GameSession>(id);

            if (entity == null)
            {
                Response.StatusCode = 404;
                Response.StatusDescription = "Character was not found"; //TODO: error response?
            }
                
            _ravenSession.Delete(entity);

            return new CollectionJsonResponse<GameSession>(entity);
        }
        
        //PUT
        public CollectionJsonResponse Update(CollectionJsonReader<GameSession> reader) //TODO this has to be a template representation!
        {
            var entity = reader.Entity;
            if (entity == null)
            {
                Response.StatusCode = 404;
                Response.StatusDescription = "Character was not found"; //TODO: error response?
            }

            _ravenSession.Store(entity);
            return new CollectionJsonResponse<GameSession>(entity);
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
