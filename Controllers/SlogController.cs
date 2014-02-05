using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Routing;
using CollectionJsonExtended.Client;
using CollectionJsonExtended.Client.Attributes;
using CollectionJsonExtended.Core;
using MedienKultur.Gurps.Models;
using Raven.Client;

namespace MedienKultur.Gurps.Controllers
{
    
    public class SlogController : Controller
    {
        const string BaseUri = "api/slog";
        readonly IDocumentSession _ravenSession;

        public SlogController(IDocumentSession ravenSession)
        {
            _ravenSession = ravenSession;
        }


        [CollectionJsonRoute(Is.Base, BaseUri)]
        public CollectionJsonResult<Slog> Get()
        {
            //TODO: we need to support not getting items (CollectionJsonResult<>() empty signature)...
            var models = _ravenSession.Query<Slog>();
            return new CollectionJsonResult<Slog>(models);
        }
        
        [CollectionJsonRoute(Is.Query, BaseUri + "/{id}")]
        public CollectionJsonResult<Slog> FilterEntries(int id, int characterId)
        {
            var models = _ravenSession.Query<Slog>();
            return new CollectionJsonResult<Slog>(models);
        }

        [CollectionJsonRoute(Is.Item, BaseUri + "/{id}")]
        public CollectionJsonResult<Slog> Get(int id)
        {
            var model = _ravenSession.Load<Slog>(id);
            if (model == null)
            {
                Response.StatusCode = 404;
                Response.StatusDescription = "Slog was not found";
            }

            return new CollectionJsonResult<Slog>(model);
        }

        

        //
    }
}
