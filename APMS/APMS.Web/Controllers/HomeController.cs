using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APMS.Business.Web;

namespace APMS.Web.Controllers
{
    public class HomeController : Controller
    {
        [AuthorizationFilter]
        [HttpGet]
        public ActionResult Index()
        {
            IHomeWeb homeWeb = new HomeWeb();
            DataAccess.Account account = (DataAccess.Account)Session["Account"];
            HomeViewModel model = homeWeb.GetDeviceListByAccountId(account.AccountId);
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}