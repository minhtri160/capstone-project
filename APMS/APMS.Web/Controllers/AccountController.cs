using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APMS.Business.Web;
using APMS.DataAccess;
using System.Web.Routing;

namespace APMS.Web.Controllers
{
    public class AuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        //private UserType? _role;
        public AuthorizationFilter()
        {
            //_role = null;
        }
        //public AuthorizationFilter(UserType role)
        //{
        //    _role = role;
        //}
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // Don't check for authorization as AllowAnonymous filter is applied to the action or controller
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;
            }
            // Check for authorization
            if (HttpContext.Current.Session["Account"] == null)
            {
                if (HttpContext.Current.Request.Cookies["Account"] == null)
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Account" }, { "action", "Login" } });
                else
                {
                    IAccountBusiness loginWeb = new AccountBusiness();
                    LoginViewModel model = new LoginViewModel();
                    model.Username = HttpContext.Current.Request.Cookies["Account"]["Username"];
                    model.Password = HttpContext.Current.Request.Cookies["Account"]["Password"];
                    Account account = loginWeb.CheckUserByCookie(model);
                    if (account == null)
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Account" }, { "action", "Login" } });
                    else
                        HttpContext.Current.Session["Account"] = account;
                }
                    

            }
        }
    }

    
    public class AccountController : Controller
    {
        

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string token=null)
        {
            LoginWebViewModel model = new LoginWebViewModel();
            if (token == null)
            {
                return View(model);
            }
            IAccountBusiness login = new AccountBusiness();


            Account account = login.CheckUserByToken(token);
            if (account == null)
            {
                model.Password = "";
                model.ErrMessage = "Token is invalid!";
                return View(model);
            }
            Session["Account"] = account;
            HttpCookie cookie = login.CreateCookie(account.AccountId);
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginWebViewModel model)
        {   
            IAccountBusiness login = new AccountBusiness();
            if (model.Username == null || model.Password == null)
            {
                model.Password = "";
                model.ErrMessage = "Username or Password can't be empty!";
                return View(model);
            }
            Account account = login.CheckUserWeb(model);
            if (account == null)
            {
                model.Password = "";
                model.ErrMessage = "Username or Password is wrong!";
                return View(model);
            }
            Session["Account"] = account;
            HttpCookie cookie = login.CreateCookie(account.AccountId);
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index", "Home");
        }

        [AuthorizationFilter]
        [HttpGet]
        public ActionResult Logout()
        {
            Session["Account"] = null;
            HttpCookie cookie = Request.Cookies["Account"];
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);
            return RedirectToAction("Login");
        }
    }
}