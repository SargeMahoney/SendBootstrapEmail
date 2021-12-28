using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SendBootstrapEmail.TemplateServices
{
    public class TemplateService : ITemplateService
    {

        private IRazorViewEngine _razorViewEngine;   
        private ITempDataProvider _tempDataProvider;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TemplateService(
            IRazorViewEngine engine,    
            ITempDataProvider tempDataProvider, 
            IServiceScopeFactory serviceScopeFactory)
        {
            this._razorViewEngine = engine;        
            this._tempDataProvider = tempDataProvider;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<string> GetTemplateHtmlAsStringAsync<T>(
          string viewName, T model) where T : class, new()
        {

            using (var scope = _serviceScopeFactory.CreateScope())
            {

                var httpContext = new DefaultHttpContext() { RequestServices = scope.ServiceProvider };



                var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

                using (var sw = new StringWriter())
                {
                    var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);

                    if (viewResult.View == null)
                    {
                        return string.Empty;
                    }

                    var metadataProvider = new EmptyModelMetadataProvider();
                    var msDictionary = new ModelStateDictionary();
                    var viewDataDictionary = new ViewDataDictionary(metadataProvider, msDictionary);

                    viewDataDictionary.Model = model;

                    var tempDictionary = new TempDataDictionary(actionContext.HttpContext, _tempDataProvider);
                    var viewContext = new ViewContext(
                        actionContext,
                        viewResult.View,
                        viewDataDictionary,
                        tempDictionary,
                        sw,
                        new HtmlHelperOptions()
                    );

                    await viewResult.View.RenderAsync(viewContext);
                    return sw.ToString();
                }
            }

        }
    }
}
