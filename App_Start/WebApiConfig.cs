using System.Web.Http;

namespace MedienKultur.Gurps.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            //jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //http://www.asp.net/web-api/overview/formats-and-model-binding/json-and-xml-serialization
            //var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            //json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //json.SerializerSettings.Formatting = Formatting.Indented; 

            config.Routes.MapHttpRoute(
                name: "UploadApi",
                routeTemplate: "api/{type}/upload",
                defaults: new { controller = "jupload" }
            );

            //config.Routes.MapHttpRoute(
            //    name: "CharacterApi",
            //    routeTemplate: "api/character/{id}",
            //    defaults: new {controller = "jcharacter", id = RouteParameter.Optional}
            //);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional}
            );
        }
    }
}
