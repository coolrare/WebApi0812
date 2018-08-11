using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using WebApi2.Models;

namespace WebApi2.Controllers
{
    [RoutePrefix("accounts")]
    public class AccountsController : ApiController
    {
        [HttpPost, Route("login")]
        public IHttpActionResult Login(LoginViewModel user)
        {
            if (user.Username == "will" && user.Password == "123")
            {
                // 這段程式碼預設會寫入 Location 標頭，所以最好要回應 HTTP 302
                FormsAuthentication.RedirectFromLoginPage(user.Username, false);
                return RedirectToRoute(nameof(GetProfile), null);
            }

            return BadRequest("帳號或密碼錯誤");
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
                Username = User.Identity.Name
            });
        }
    }
}
