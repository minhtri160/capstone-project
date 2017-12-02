using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APMS.DataAccess;

namespace APMS.Business.Web
{
    public interface INotificationBusiness
    {
        void SendWarningSensorStateNotification(Sensor sensor);
        void SendWarningDisconnectionNotification(string accountId);
        void SendWarningNotRespondNotification(Device device);
        int GetNumberOfUnreadNoticationByAccountId(string accountId);
        List<Notification> GetNotiListByAccountIdAndState(string accountId, int state);
        List<Notification> GetNotiListByAccountId(string accountId);
        Notification GetNotiByIdAndSetReadState(int Id);

    }
}
