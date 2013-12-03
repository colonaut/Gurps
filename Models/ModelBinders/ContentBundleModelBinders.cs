using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.Mvc;

namespace MedienKultur.Gurps.Models.ModelBinders
{
    public class ContentBundleContentModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            //var typeValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + ".ContentType") ?? new ValueProviderResult(ContentBundle.ContentType.Text, "Text", CultureInfo.InvariantCulture);

            //var type = Type.GetType(
            //    "MedienKultur.Gurps.Models.ContentBundle+ " + (string)typeValue.ConvertTo(typeof(string)),
            //    true
            //);
            
            
            var model = Activator.CreateInstance(modelType);
            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, modelType);
            return model;
        }
    }

}