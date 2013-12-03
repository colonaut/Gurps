using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using MedienKultur.Gurps.App_Start;
using MedienKultur.Gurps.Models;
using MedienKultur.Gurps.Models.ModelBinders;

namespace MedienKultur.Gurps
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            //WebApiConfig.Register(GlobalConfiguration.Configuration); //Not NEEDED due to registration in normal route config (each controller)

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //BundleConfig.RegisterBundles(BundleTable.Bundles); we do not use system.web.optimization (not included)

            //AuthConfig.RegisterAuth(); we do not use auth config.

            ModelBinders.Binders.Add(typeof(ContentBundle.Content), new ContentBundleContentModelBinder());
        }
    }
}