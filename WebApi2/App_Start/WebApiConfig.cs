using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApi2.Controllers;

namespace WebApi2
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 設定和服務
            config.Filters.Add(new MyExceptionAttribute());

            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;

            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();


            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
