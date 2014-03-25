using System.Web.Mvc;
using CollectionJsonExtended.Client;
using CollectionJsonExtended.Client.Attributes;
using CollectionJsonExtended.Core;
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
        public CollectionJsonResult<Slog> Get()
        {
            //TODO: we need to support not getting items (CollectionJsonResult<>() empty signature)...
            var models = _ravenSession.Query<Slog>();
            return new CollectionJsonResult<Slog>(models);
        }

        [CollectionJsonRoute(Is.Query, "{id}")]
        public CollectionJsonResult<Slog> Filter(int id, int characterId)
        {
            var models = _ravenSession.Query<Slog>();
            return new CollectionJsonResult<Slog>(models);
        }

        [CollectionJsonRoute(Is.Item, "{id}")]
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
