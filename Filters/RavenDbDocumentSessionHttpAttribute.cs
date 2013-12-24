using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Raven.Client;
using StructureMap;

namespace MedienKultur.Gurps.Filters
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class RavenDbDocumentSessionHttpAttribute : ActionFilterAttribute, IActionFilter
    {

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // pre-processing
        }
        
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            //post-processing
            if (actionExecutedContext.Exception != null)
                return; // don't commit changes if an exception was thrown

            using (var session = ObjectFactory.GetInstance<IDocumentSession>())
                session.SaveChanges();
        }
    }
}