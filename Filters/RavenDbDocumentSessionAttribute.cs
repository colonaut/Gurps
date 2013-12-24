using System;
using System.Web.Mvc;
using Raven.Client;
using StructureMap;

namespace MedienKultur.Gurps.Filters
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class RavenDbDocumentSessionAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
			// pre-processing
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //post-processing
			if (filterContext.Exception != null)
                return; // don't commit changes if an exception was thrown

            using (var session = ObjectFactory.GetInstance<IDocumentSession>())
                session.SaveChanges();
        }
    }
}