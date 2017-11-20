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
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Account" }, { "action", "Login" } });
            }
        }
    }

    [AuthorizationFilter]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            LoginWebViewModel model = new LoginWebViewModel();
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginWebViewModel model)
        {   
            ILoginWeb login = new LoginWeb();
            Account account = login.CheckUser(model);
            if (account == null)
            {
                model.Password = "";
                model.ErrMessage = "Username or Password is wrong!!!";
                return View(model);
            }
            Session["Account"] = account;
            return RedirectToAction("Index", "Home");
        }
    }
}