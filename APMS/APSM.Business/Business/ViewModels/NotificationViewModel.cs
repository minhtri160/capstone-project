using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APMS.Business.Web
{
    public class NotificationViewModel
    {
        public NotificationViewModel()
        {
            NotiList = new List<DataAccess.Notification>();
        }
        public int UnreadNotiAmount { get; set; }
        public List<DataAccess.Notification> NotiList { get; set; }
    }

    public class DetailNotificationViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Time { get; set; }
    }
}