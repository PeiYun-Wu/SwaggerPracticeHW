//using ELogCMSGateway.Services;
using CommonModule;
using Newtonsoft.Json.Serialization;
using PennyTest2.Filter;
using PennyTest2.Services;
using Swashbuckle.Application;
using System.Configuration;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PennyTest2
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)//前置設定
        {
            // Web API 設定和服務
            GlobalConfiguration.Configuration.Formatters.Clear();
            //強制回應Json
            var jsonFormatter = new JsonMediaTypeFormatter();
            jsonFormatter.SupportedMediaTypes.Clear();
            jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("multipart/form-data"));
            GlobalConfiguration.Configuration.Formatters.Add(jsonFormatter);
            // 將所有output的model item顯示為首字小寫的駝峰設計
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            // Web API 路由
            config.MapHttpAttributeRoutes();
            if (ConfigurationManager.AppSettings["TestMode"] != null)
            {
                config.Routes.MapHttpRoute(
                   name: "Swagger API",
                   routeTemplate: string.Empty,
                   defaults: null,
                   constraints: null,
                   handler: new RedirectHandler(SwaggerDocsConfig.DefaultRootUrlResolver, "swagger/ui/index"));

                // Access-Control-Allow-Origin機制 (前端ajax在不同網段可以串接)
                config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            }

            config.Filters.Add(new MtkAuthenticateFilter());
           // config.Filters.Add(new FileUploadFilter());
            config.Filters.Add(new ApiPermitFilter(new LoginService()));
            config.Filters.Add(new BankEndFilter());
           // config.Filters.Add(new LogFilter());

            //自訂義回應文案
            //ResultStatusSingleton.GetInstance(new ResultMsgService());
        }
    }
}
