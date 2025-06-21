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
    // Helper para renderizar una vista Razor (.cshtml) como un string (HTML plano) desde el backend.
    // Útil para generar contenido HTML dinámico, por ejemplo, para exportar a PDF o enviar emails personalizados.
    public class RazorViewToString
    {
        // Servicios necesarios para renderizar la vista
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IActionContextAccessor _actionContextAccessor;

        // Constructor: inyección de dependencias de MVC
        public RazorViewToString(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IActionContextAccessor actionContextAccessor)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _actionContextAccessor = actionContextAccessor;
        }

        // Método principal: renderiza una vista Razor (por nombre) y devuelve el HTML como string
        public async Task<string> RenderViewAsync<TModel>(string viewName, TModel model)
        {
            // Obtiene el contexto actual de la acción (necesario para la vista)
            var actionContext = _actionContextAccessor.ActionContext
                                 ?? throw new InvalidOperationException("No ActionContext disponible");

            // Busca la vista Razor por su nombre
            var viewResult = _viewEngine.FindView(actionContext, viewName, false);
            if (!viewResult.Success)
                throw new InvalidOperationException($"La vista «{viewName}» no fue encontrada.");

            // Prepara el modelo y el contexto de la vista
            var viewData = new ViewDataDictionary<TModel>(
                new EmptyModelMetadataProvider(),
                actionContext.ModelState)
            {
                Model = model
            };

            // Usa un StringWriter para capturar el HTML resultante
            await using var sw = new StringWriter();
            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewData,
                new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                sw,
                new HtmlHelperOptions()
            );

            // Renderiza la vista en el contexto y captura el HTML en el StringWriter
            await viewResult.View.RenderAsync(viewContext);
            return sw.ToString();
        }
    }
}
