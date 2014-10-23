using System.Web.Mvc;
using System.Web.UI;
using CollectionJsonExtended.Client;
using CollectionJsonExtended.Client.Attributes;
using CollectionJsonExtended.Core;
using MedienKultur.Gurps.Models;
using Raven.Client;

namespace MedienKultur.Gurps.Controllers
{

    [RoutePrefix("api/slogs")]
    public class SlogController : Controller
    {
        private readonly IDocumentSession _ravenSession;

        public SlogController(IDocumentSession ravenSession)
        {
            _ravenSession = ravenSession;
        }


        [CollectionJsonRoute(Is.Base)]
        public CollectionJsonResult<SlogEntry> Get()
        {
            //TODO: we need to support not getting items (CollectionJsonResult<>() empty signature)...
            var models = _ravenSession.Query<SlogEntry>();
            return new CollectionJsonResult<SlogEntry>(models);
        }

        //[CollectionJsonRoute(Is.Query, "{characerId}")]
        //public CollectionJsonResult<SlogEntry> FilterByCharacter(int characterId)
        //{
        //    var models = _ravenSession.Query<SlogEntry>();
        //    return new CollectionJsonResult<SlogEntry>(models);
        //}

        //[CollectionJsonRoute(Is.Query, "{gameSessionId}")]
        //public CollectionJsonResult<SlogEntry> FilterByGameSession(int gameSessionId)
        //{
        //    var models = _ravenSession.Query<SlogEntry>();
        //    return new CollectionJsonResult<SlogEntry>(models);
        //}

        [CollectionJsonRoute(Is.Item, "{id}")]
        public CollectionJsonResult<SlogEntry> Get(int id)
        {
            var model = _ravenSession.Load<SlogEntry>(id);
            if (model == null)
            {
                Response.StatusCode = 404;
                Response.StatusDescription = "Slog was not found";
            }
            return new CollectionJsonResult<SlogEntry>(model);
        }

        //POST
        [CollectionJsonRoute(Is.Create)]
        public CollectionJsonResult<SlogEntry> Create(CollectionJsonReader<SlogEntry> template)
        {
            var entity = template.Entity;
            entity.User = new UserReference(this.GetApplicationUser(_ravenSession));
            _ravenSession.Store(entity);
            return new CollectionJsonResult<SlogEntry>(entity);
        }

    }
}
