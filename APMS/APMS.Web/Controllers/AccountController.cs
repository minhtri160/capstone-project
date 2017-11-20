using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APMS.Business.Web;
using APMS.DataAccess;

namespace APMS.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            LoginWebViewModel model = new LoginWebViewModel();
            return View(model);
        }

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