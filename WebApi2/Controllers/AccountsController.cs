using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using WebApi2.Models;

namespace WebApi2.Controllers
{
    [RoutePrefix("accounts")]
    public class AccountsController : ApiController
    {
        [HttpPost, Route("login")]
        public HttpResponseMessage Login(LoginViewModel user)
        {
            if (user.Username == "will" && user.Password == "123")
            {
                string role = "Admin";
                bool isPersistent = false;

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                    user.Username,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    isPersistent,
                    role,
                    FormsAuthentication.FormsCookiePath);

                // Encrypt the ticket.
                string encTicket = FormsAuthentication.Encrypt(ticket);

                // Create the cookie.
                var resp = new HttpResponseMessage();
                resp.StatusCode = HttpStatusCode.NoContent;

                var cookie = new CookieHeaderValue(FormsAuthentication.FormsCookieName, encTicket);
                cookie.Expires = DateTimeOffset.Now.AddDays(1);
                cookie.Domain = Request.RequestUri.Host;
                cookie.Path = "/";
                resp.Headers.AddCookies(new CookieHeaderValue[] { cookie });

                return resp;
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, "帳號或密碼錯誤");
        }

        [HttpGet, Route("logout")]
        public void Logout()
        {
            FormsAuthentication.SignOut();
        }

        [HttpGet, Route("profile", Name = nameof(GetProfile))]
        public IHttpActionResult GetProfile()
        {
            return Ok(new
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                Username = User.Identity.Name,
                IsAdminRole = User.IsInRole("Admin")
            });
        }
    }
}
