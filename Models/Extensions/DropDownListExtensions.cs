using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MedienKultur.Gurps.Models.Extensions
{
    public static class DropDownListExtensions
    {
        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                       Expression<Func<TModel, TProperty>> expression) //extend the general DropDownList for usage without giving the selectlist item
        {
            var typeOfProperty = expression.ReturnType;
            
            if (typeOfProperty.IsEnum)
                return htmlHelper.DropDownListFor(expression, new SelectList(Enum.GetValues(typeOfProperty)));
            
            throw new ArgumentException(string.Format("TProperty is {0}. Currently the only supported types are: enum", typeOfProperty));
        }

    }
}