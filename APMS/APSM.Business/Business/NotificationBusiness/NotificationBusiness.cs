using System;
using APMS.DataAccess;
using APMS.Business.Dictionary;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.IO;

namespace APMS.Business.Web
{
    public class NotificationBusiness : INotificationBusiness
    {

        public static Queue<Notification> notificationRemainingQueue = new Queue<Notification>();

        private IRepository<Notification> notificationRepository = new Repository<Notification>();
        private IRepository<Account> accountRepository = new Repository<Account>();

        private void SendSMSNotification(string phone, string content)
        {
            string APIKey = "F810A4D8F7369142011034644DA13A";
            string SecretKey = "44751593112870EEE56F64F6794C93";

            string RequestUrl = "http://rest.esms.vn/MainService.svc/json/SendMultipleMessage_V4_get?Phone=" 
                                + phone + "&Content=" 
                                + content + "&ApiKey=" 
                                + APIKey + "&SecretKey=" 
                                + SecretKey + "&IsUnicode=0&SmsType=4";
            
            
            Uri address = new Uri(RequestUrl);
            HttpWebRequest request;
            HttpWebResponse response = null;
            StreamReader reader;
            if (address == null) { throw new ArgumentNullException("address"); }
            try
            {
                request = WebRequest.Create(address) as HttpWebRequest;
                request.UserAgent = ".NET Sample";
                request.KeepAlive = false;
                request.Timeout = 15 * 1000;
                response = request.GetResponse() as HttpWebResponse;
                if (request.HaveResponse == true && response != null)
                {
                    reader = new StreamReader(response.GetResponseStream());
                    string result = reader.ReadToEnd();
                    result = result.Replace("</string>", "");
                    Console.WriteLine(result);
                }
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)wex.Response)
                    {
                        Console.WriteLine(
                            "The server returned '{0}' with the status code {1} ({2:d}).",
                            errorResponse.StatusDescription, errorResponse.StatusCode,
                            errorResponse.StatusCode);
                    }
                }
            }
            finally
            {
                if (response != null) { response.Close(); }
            }
        }
        private void SendNotification(Notification notification)
        {
            try
            {
                Account acc = accountRepository.GetAll().Where(x => x.AccountId.Equals(notification.AccountId)).FirstOrDefault();
                
                notificationRepository.Insert(notification);
                //SendSMSNotification(acc.Phone, notification.Content);
                notification.Account = acc;
                notificationRemainingQueue.Enqueue(notification);
            }
            catch
            {
                Console.WriteLine("Error from send notification");
            }
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

        public void SendWarningSensorStateNotification(Sensor sensor)
        {
            DataAccess.Device currentDevice = sensor.Device;
            DataAccess.Notification notification = new DataAccess.Notification();

            notification.AccountId = currentDevice.AccountId;
            notification.Title = "Warning About " + currentDevice.DeviceName + " in " + currentDevice.Position;
            notification.Time = DateTime.Now;
            string warningMessage = GetWarningMessage((int)sensor.WarningState);

            string addString1 = "";
            if (currentDevice.Position != null)
            {
                if (!currentDevice.Position.Trim().Equals(""))
                {
                    addString1 = " in " + currentDevice.Position;
                }
            }
            notification.Content = currentDevice.DeviceName + addString1 + " has a warning about: " + warningMessage + "!";
            notification.State = (int)NotificationState.Unread;
            SendNotification(notification);
        }

        public void SendWarningDisconnectionNotification(string accountId)
        {
            DataAccess.Notification notification = new DataAccess.Notification();
            notification.AccountId = accountId;
            notification.Title = "Warning About Disconnection";
            notification.Time = DateTime.Now;
            notification.Content = "Center Control Unit of your house has been disconnected with server on " +
                                    notification.Time.ToShortDateString() + " " + notification.Time.ToShortTimeString();
            notification.State = (int)NotificationState.Unread;
            SendNotification(notification);
        }

        public void SendWarningNotRespondNotification(Device device)
        {
            DataAccess.Notification notification = new DataAccess.Notification();
            notification.AccountId = device.AccountId;
            string addString1 = "";
            if (device.Position != null)
            {
                if (!device.Position.Trim().Equals(""))
                {
                    addString1 = " in " + device.Position;
                }
            }
            notification.Title = "Warning About Not Responding Of Device";
            notification.Time = DateTime.Now;
            notification.Content = device.DeviceName + addString1 + " of your house was not responding to server on " +
                                    notification.Time.ToShortDateString() + " " + notification.Time.ToShortTimeString();
            notification.State = (int)NotificationState.Unread;
            SendNotification(notification);
        }

        public int GetNumberOfUnreadNoticationByAccountId(string accountId)
        {
            List<DataAccess.Notification> notiList = GetNotiListByAccountIdAndState(accountId, (int)Dictionary.NotificationState.Unread);
            return notiList.Count();
        }

        public List<Notification> GetNotiListByAccountIdAndState(string accountId, int state)
        {
            List<DataAccess.Notification> notiList = notificationRepository.GetAll().Where(x => x.AccountId.Equals(accountId)
                                                                && x.State == state).ToList();
            return notiList;
        }

        public List<Notification> GetNotiListByAccountId(string accountId)
        {
            List<DataAccess.Notification> notiList = notificationRepository.GetAll().Where(x => x.AccountId.Equals(accountId)).OrderByDescending(x => x.Time).ToList();
            return notiList;
        }

        public Notification GetNotiByIdAndSetReadState(int id)
        {
            DataAccess.Notification noti = notificationRepository.GetAll().Where(x => x.Id == id).FirstOrDefault();
            if (noti != null)
            {
                noti.State = (int)Dictionary.NotificationState.Read;
                notificationRepository.Update(noti);
            }
            return noti;
        }
    }
}
