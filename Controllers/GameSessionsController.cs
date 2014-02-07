﻿using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using CollectionJsonExtended.Client;
using CollectionJsonExtended.Client.Attributes;
using CollectionJsonExtended.Core;
using MedienKultur.Gurps.Models;
using Raven.Client;

namespace MedienKultur.Gurps.Controllers
{
    
    [RoutePrefix("foo/bar")]
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

            //routes.MapRoute(
            //    BaseUri + "/{id} DELETE",
            //    BaseUri + "/{id}",
            //    new { controller = "GameSessions", action = "Delete" },
            //    new { httpMethod = new HttpMethodConstraint("DELETE") }
            //);


            routes.MapRoute(
                BaseUri + "/{id} PUT",
                BaseUri + "/{id}",
                new { controller = "GameSessions", action = "Update" },
                new { httpMethod = new HttpMethodConstraint("PUT") }
            );
            

            //routes.MapRoute(
            //    BaseUri + "/{id} GET",
            //    BaseUri + "/{id}",
            //    new { controller = "GameSessions", action = "Get" }
                //,new { httpMethod = new HttpMethodConstraint("GET") }
            //);
            
            //routes.MapRoute(
            //    BaseUri + " QUERY",
            //    BaseUri + "",
            //    new { controller = "GameSessions", action = "Query" },
            //    new { httpMethod = new HttpMethodConstraint("QUERY") }
            //);
            
            //routes.MapRoute(
            //    BaseUri + " POST",
            //    BaseUri,
            //    new { controller = "GameSessions", action = "Create" },
            //    new {httpMethod = new HttpMethodConstraint("POST")}
            //);
            
        }
        #endregion

        //a link
        [CollectionJsonRoute(Is.BaseLink, "api/gamesessions/feed")]
        public CollectionJsonResult<GameSession> GetNews()
        {
            var models = _ravenSession.Query<GameSession>()
                .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite());
            //.AsEnumerable();
            return new CollectionJsonResult<GameSession>(models);
        }

        //an item link
        [CollectionJsonRoute(Is.ItemLink, "api/gamesessions/{id}/avatar")]
        public CollectionJsonResult<GameSession> GetImage(int id)
        {
            var model = _ravenSession.Load<GameSession>(id);
            //.AsEnumerable();
            return new CollectionJsonResult<GameSession>(model);
        }

        //GET base
        [CollectionJsonRoute(Is.Base, "api/gamesessions")]
        public CollectionJsonResult<GameSession> Get()
        {
            var models = _ravenSession.Query<GameSession>()
                .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite());
            //.AsEnumerable();
            return new CollectionJsonResult<GameSession>(models);
        }

        //GET query
        [CollectionJsonRoute(Is.Query, "api/gamesessions/search")] //querystring works just with method signature
        public CollectionJsonResult<GameSession> ByDate(string dateTime)
        {
            var models = _ravenSession.Query<GameSession>()
                .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite());
            //.AsEnumerable();
            return new CollectionJsonResult<GameSession>(models);
        }

        //query by character id
        [CollectionJsonRoute(Is.Query, "api/gamesessions/search")]
        public CollectionJsonResult<GameSession> ByCharacter(int character)
        {
            var models = _ravenSession.Query<GameSession>()
                .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
                .Where(s => s.Characters.Any(c => c.CharacterId == character));
            //.AsEnumerable();
            return new CollectionJsonResult<GameSession>(models);
        }

        //GET item
        [CollectionJsonRoute(Is.Item, "api/gamesessions/{id:int}")]
        public CollectionJsonResult<GameSession> Get(int id)
        {
            var model = _ravenSession.Load<GameSession>(id);
            //if (model == null)
            //{
            //    Response.StatusCode = 404; //therfore we need an error response (wrapped in response!)
            //    Response.StatusDescription = "GameSession was not found";
            //} now done from result...?

            return new CollectionJsonResult<GameSession>(model);
        }

        //POST
        int tmpId = 1;
        [CollectionJsonRoute(Is.Create, "api/gamesessions")]
        public CollectionJsonResult<GameSession> Create(CollectionJsonReader<GameSession> reader)
        {
            
            var entity = new GameSession {SlogId = 1};
            _ravenSession.Store(entity);

            return new CollectionJsonResult<GameSession>(entity);
        }

        //DELETE
        [CollectionJsonTask(Do.Delete, "api/gamesessions/{id:int}")]
        //[RouteCollectionJsonDelete("api/gamesessions/{id:int}")]
        public CollectionJsonResult<GameSession> Delete(int id)
        {
            var entity = _ravenSession.Load<GameSession>(id);

            if (entity == null)
            {
                Response.StatusCode = 404;
                Response.StatusDescription = "Character was not found"; //TODO: error response?
            }
                
            _ravenSession.Delete(entity);

            return new CollectionJsonResult<GameSession>(entity);
        }
        
        //PUT
        [CollectionJsonRoute(Is.Update, "api/gamesessions")]
        public CollectionJsonResult<GameSession> Update(CollectionJsonReader<GameSession> reader) //TODO this has to be a template representation!
        {
            var entity = reader.Entity;
            if (entity == null)
            {
                Response.StatusCode = 404;
                Response.StatusDescription = "Character was not found"; //TODO: error response?
            }

            _ravenSession.Store(entity);
            return new CollectionJsonResult<GameSession>(entity);
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
