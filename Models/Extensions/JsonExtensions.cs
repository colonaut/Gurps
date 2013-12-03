using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MedienKultur.Gurps.Models.Extensions
{
    //TODO: put this to CollectionJson?
    
    public interface IJsonSerializeable
    {

    }

    public static class JsonExtensions
    {
        static readonly JsonSerializerSettings settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

        public static MvcHtmlString ToJson<TModel>(this TModel model) where TModel : class, IJsonSerializeable
        {
            return MvcHtmlString.Create(JsonConvert.SerializeObject(model, Formatting.None, settings));
        }

        public static MvcHtmlString ToJson<TModel>(this IEnumerable<TModel> model) where TModel : class, IJsonSerializeable
        {
            return MvcHtmlString.Create(JsonConvert.SerializeObject(model, Formatting.None, settings));
        }


    }
}