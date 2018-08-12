using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using WebApi2.Models;

namespace WebApi2.Controllers
{
    public class MyExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Content = new ObjectContent<MyHttpError>(new MyHttpError()
                {
                    Error_Code = 1,
                    Error_Message = actionExecutedContext.Exception.Message
                },
                GlobalConfiguration.Configuration.Formatters.JsonFormatter)
            };
        }
    }
}