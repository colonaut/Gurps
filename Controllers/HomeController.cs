using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MedienKultur.Gurps.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            //CultureInfo ci = new CultureInfo("fr-FR");
            //var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            //foreach (var cultureInfo in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            //{

            //    ViewBag.Message += "{\"" + cultureInfo + "\":";
            //    ViewBag.Message += string.Format(CultureInfo.InvariantCulture, "{0}",
            //                                     JsonConvert.SerializeObject(cultureInfo.DateTimeFormat, Formatting.None, settings));
            //    ViewBag.Message += "}";
            //}



            return View(new MedienKultur.RavenDBUnitOfWork.App_Start.StartUp());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
