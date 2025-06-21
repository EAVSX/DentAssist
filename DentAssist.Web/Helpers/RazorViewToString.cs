// Helpers/RazorViewToString.cs
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;

namespace DentAssist.Web.Helpers
{
    public class RazorViewToString
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IActionContextAccessor _actionContextAccessor;

        public RazorViewToString(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IActionContextAccessor actionContextAccessor)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _actionContextAccessor = actionContextAccessor;
        }

        public async Task<string> RenderViewAsync<TModel>(string viewName, TModel model)
        {
            var actionContext = _actionContextAccessor.ActionContext
                                 ?? throw new InvalidOperationException("No ActionContext disponible");

            var viewResult = _viewEngine.FindView(actionContext, viewName, false);
            if (!viewResult.Success)
                throw new InvalidOperationException($"La vista «{viewName}» no fue encontrada.");

            var viewData = new ViewDataDictionary<TModel>(
                new EmptyModelMetadataProvider(),
                actionContext.ModelState)
            {
                Model = model
            };

            await using var sw = new StringWriter();
            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewData,
                new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                sw,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return sw.ToString();
        }
    }
}
