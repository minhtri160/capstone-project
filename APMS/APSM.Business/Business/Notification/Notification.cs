using System;
using APMS.DataAccess;
using APMS.Business.Dictionary;
using System.Linq;

namespace APMS.Business.Web
{
    public class Notification
    {
        private void SendNotification(DataAccess.Notification notification)
        {
            IRepository<DataAccess.Notification> notificationRepository = new Repository<DataAccess.Notification>();

            notificationRepository.Insert(notification);
            //  send notification to android here
        }

        private string GetWarningMessage(int warningState)
        {
            string warningMessage = "";
            if (warningState == (int)WarningState.FanSlow)
                warningMessage = "Slow Spinning";
            else if (warningState == (int)WarningState.GasLeakage)
                warningMessage = "Gas Leakage Detected";
            else if (warningState == (int)WarningState.HasFire)
                warningMessage = "Fire Detected";
            else if (warningState == (int)WarningState.HasMotion)
                warningMessage = "Motion Detected";
            else if (warningState == (int)WarningState.HasSmoke)
                warningMessage = "Smoke Detected";
            else if (warningState == (int)WarningState.HighVolta)
                warningMessage = "High Voltage";
            else if (warningState == (int)WarningState.LampNotWork)
                warningMessage = "Lamp Not Work";
            else if (warningState == (int)WarningState.LowTemp)
                warningMessage = "Low Temperature";
            else if (warningState == (int)WarningState.LowVolta)
                warningMessage = "Low Voltage";
            else if (warningState == (int)WarningState.TooMoist)
                warningMessage = "Too Moist";
            else if (warningState == (int)WarningState.Others)
                warningMessage = "Others";
            else if (warningState == (int)WarningState.OverCurrent)
                warningMessage = "Over Current";
            else if (warningState == (int)WarningState.OverTemp)
                warningMessage = "Over Temperature";
            else if (warningState == (int)WarningState.TooDry)
                warningMessage = "Too Dry";
            return warningMessage;
        }

        public void SendWarningSensorStateNotification(DataAccess.Sensor sensor)
        {
            DataAccess.Device currentDevice = sensor.Device;
            DataAccess.Notification notification = new DataAccess.Notification();

            notification.AccountId = currentDevice.AccountId;
            notification.Title = "Warning About " + currentDevice.DeviceName + " in " + currentDevice.Position;
            notification.Time = DateTime.Now;
            string warningMessage = GetWarningMessage((int)sensor.WarningState);
            
            warningMessage = "Slow Speed";
            notification.Content = currentDevice.DeviceName + " in " + currentDevice.Position + " has a warning about: " + warningMessage + "!";
            notification.State = (int)NotificationState.Unread;
            SendNotification(notification);
        }

        public void SendWarningDisconnectionNotification(string AccountId)
        {
            DataAccess.Notification notification = new DataAccess.Notification();
            notification.AccountId = AccountId;
            notification.Title = "Warning About Disconnection";
            notification.Time = DateTime.Now;
            notification.Content = "Center Control Unit of your house has been disconnected with server on " + 
                                    notification.Time.ToShortDateString() + " " + notification.Time.ToShortTimeString();
            notification.State = (int)NotificationState.Unread;
            SendNotification(notification);
        }
    }
}
