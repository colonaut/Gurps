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
    [RoutePrefix("api/characters")]
    public class CharactersController : Controller
    {
        readonly IDocumentSession _ravenSession;

        public CharactersController(IDocumentSession ravenSession)
        {
            _ravenSession = ravenSession;
        }


        public CollectionJsonResult<GurpsCharacter> Query()
        {
            var models = _ravenSession.Query<GurpsCharacter>();
            return new CollectionJsonResult<GurpsCharacter>(models);
        }

        //collection of characters
        [CollectionJsonRoute(Is.Base, RouteName = "CharacterCollection")]
        public CollectionJsonResult<GurpsCharacter> Get()
        {
            var models = _ravenSession.Query<GurpsCharacter>()
                .Customize(q => q.WaitForNonStaleResults());
            
            return new CollectionJsonResult<GurpsCharacter>(models);
        }

        //collection of 1 character
        [CollectionJsonRoute(Is.Item, "{id}", RouteName = "Character")]
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
