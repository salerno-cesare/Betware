using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace ECMS.Helpers
{
    public static class HtmlHelpers
    {
        public static IHtmlContent ErrorCssClassFor(this IHtmlHelper htmlHelper, string FieldKey)
        {
            if (!string.IsNullOrEmpty(FieldKey))
            {
                var state = htmlHelper.ViewData.ModelState.GetFieldValidationState(FieldKey);
                if (state == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                {
                    return new HtmlString("has-error");
                }
            }
            return null;
        }
    }
}
