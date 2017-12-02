using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APMS.Business.Web;

namespace APMS.Web.Controllers
{
    [AuthorizationFilter]

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            IDeviceBusiness deviveBusiness = new DeviceBusiness();
            Business.Web.INotificationBusiness notification = new Business.Web.NotificationBusiness();
            DataAccess.Account account = (DataAccess.Account)Session["Account"];
            HomeViewModel model = new HomeViewModel();
            model.DeviceList = deviveBusiness.GetDeviceListByAccountId(account.AccountId);
            model.NewNotificationAmount = notification.GetNumberOfUnreadNoticationByAccountId(account.AccountId);
            return View(model);
        }
        

        public ActionResult Notification()
        {
            Business.Web.INotificationBusiness notification = new Business.Web.NotificationBusiness();
            DataAccess.Account account = (DataAccess.Account)Session["Account"];
            NotificationViewModel model = new NotificationViewModel();
            model.NotiList = notification.GetNotiListByAccountId(account.AccountId);
            model.UnreadNotiAmount = notification.GetNumberOfUnreadNoticationByAccountId(account.AccountId);
            return PartialView(model);
        }

        public ActionResult DetailNotification(int id)
        {
            INotificationBusiness notificationBusiness = new NotificationBusiness();
            DetailNotificationViewModel model = new DetailNotificationViewModel();
            DataAccess.Notification currentNotification = notificationBusiness.GetNotiByIdAndSetReadState(id);
            model.Content = currentNotification.Content;
            model.Time = currentNotification.Time.ToShortDateString() + " " + currentNotification.Time.ToShortTimeString();
            model.Title = currentNotification.Title;
            return PartialView(model);
        }

        public ActionResult MoreInfo()
        {
            return PartialView();
        }
    }
}