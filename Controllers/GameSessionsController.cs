using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebSockets;
using CollectionJsonExtended.Client;
using CollectionJsonExtended.Client.Attributes;
using CollectionJsonExtended.Core;
using MedienKultur.Gurps.Models;
using Raven.Client;

namespace MedienKultur.Gurps.Controllers
{

    [RoutePrefix("api/gamesessions")]
    public class GameSessionsController : Controller
    {
        readonly IDocumentSession _ravenSession;

        public GameSessionsController(IDocumentSession ravenSession)
        {
            _ravenSession = ravenSession;
        }

        //a link
        [CollectionJsonRoute(Is.BaseLink, "feed")]
        public CollectionJsonResult<GameSession> GetNews()
        {
            var models = _ravenSession.Query<GameSession>()
                .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite());
            //.AsEnumerable();
            return new CollectionJsonResult<GameSession>(models);
        }

        //an item link
        [CollectionJsonRoute(Is.ItemLink, "{id}/avatar")]
        public CollectionJsonResult<GameSession> GetImage(int id)
        {
            var model = _ravenSession.Load<GameSession>(id);
            //.AsEnumerable();
            return new CollectionJsonResult<GameSession>(model);
        }

        //GET base
        [CollectionJsonRoute(Is.Base)]
        public CollectionJsonResult<GameSession> Get()
        {
            var models = _ravenSession.Query<GameSession>()
                .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite());
            //.AsEnumerable();
            return new CollectionJsonResult<GameSession>(models);
        }

        //GET query
        [CollectionJsonRoute(Is.Query, "search")] //querystring works just with method signature
        public CollectionJsonResult<GameSession> ByDate(string dateTime)
        {
            var models = _ravenSession.Query<GameSession>()
                .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite());
            //.AsEnumerable();
            return new CollectionJsonResult<GameSession>(models);
        }

        //query by character id
        //[CollectionJsonRoute(Is.Query, "search")]
        //public CollectionJsonResult<GameSession> ByCharacter(int character)
        //{
        //    var models = _ravenSession.Query<GameSession>()
        //        .Customize(q => q.WaitForNonStaleResultsAsOfLastWrite())
        //        .Where(s => s.Characters.Any(c => c.CharacterId == character));
        //    //.AsEnumerable();
        //    return new CollectionJsonResult<GameSession>(models);
        //}

        //GET item
        [CollectionJsonRoute(Is.Item, "{id:int}")]
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
        [CollectionJsonRoute(Is.Create)]
        public CollectionJsonResult<GameSession> Create(CollectionJsonReader<GameSession> template)
        {
            var entity = template.Entity;
            _ravenSession.Store(entity);
            return new CollectionJsonResult<GameSession>(entity);
        }

        //DELETE
        [CollectionJsonTask(Do.Delete, "{id:int}")]
        //[RouteCollectionJsonDelete("api/gamesessions/{id:int}")]
        public CollectionJsonResult<GameSession> Delete(int id)
        {
            var entity = _ravenSession.Load<GameSession>(id);

            if (entity == null)
            {
                Response.StatusCode = 404;
                Response.StatusDescription = "GS was not found"; //TODO: error response?
            }
                
            _ravenSession.Delete(entity);

            return new CollectionJsonResult<GameSession>(entity);
        }
        
        //PUT
        [CollectionJsonRoute(Is.Update)]
        public CollectionJsonResult<GameSession> Update(CollectionJsonReader<GameSession> reader) //TODO this has to be a template representation!
        {
            var entity = reader.Entity;
            if (entity == null)
            {
                Response.StatusCode = 404;
                Response.StatusDescription = "GS was not found"; //TODO: error response?
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
