using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using APMS.Business.Web;
using System.Timers;
using APMS.Business.API;
using Microsoft.AspNet.SignalR.Hubs;
using APMS.DataAccess;

namespace APMS.Web
{
    [HubName("signalRHub")]
    public class SignalRHub : Hub
    {
        static Timer timerNotification = new Timer();
        static Timer timerCheckDevice = new Timer();
        ISensorBusiness saveSensorDataAPI = new SensorBusiness();
        INotificationBusiness notification = new NotificationBusiness();
        IDeviceBusiness deviceBusiness = new DeviceBusiness();

        
        public SignalRHub()
        {
            timerNotification.Interval = 1000;
            timerNotification.Elapsed += OnTimeEvent;
            timerNotification.Start();

            timerCheckDevice.Interval = 1000 * (int)Business.Dictionary.Time.CheckDeviceTime;
            timerCheckDevice.Elapsed += OnTimeEvent2;
            timerCheckDevice.Start();

        }

        private void OnTimeEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            Notification();
        }

        private void OnTimeEvent2(Object source, System.Timers.ElapsedEventArgs e)
        {
            CheckDevice();
        }

        private static Dictionary<string, string> raspberryConnectionIdChannelDic = new Dictionary<string, string>();
        public async Task JoinGroup(string channel, bool isRaspberry = false)
        {
            if (isRaspberry)
            {
                try
                {
                    raspberryConnectionIdChannelDic.Add(Context.ConnectionId, channel);
                }
                catch
                {

                }
            }
            await Groups.Add(Context.ConnectionId, channel);
        }

        public void PushSensorData(string channel, string value)
        {
            try
            {
                string[] valueList = value.Split(';');
                Business.Web.SensorsDataViewModel model = new SensorsDataViewModel();
                model.DeviceId = valueList[0];
                model.State = Int32.Parse(valueList[1]);
                for (int i = 2; i < valueList.Length; i++)
                {
                    var item = valueList[i];
                    string[] sensorData = item.Split(':');
                    Business.Web.SensorViewModel sensor = new Business.Web.SensorViewModel();
                    sensor.SensorId = sensorData[0];
                    sensor.Value = Double.Parse(sensorData[1]);
                    model.SensorList.Add(sensor);
                }
                SensorsWebInfoViewModel responseModel = saveSensorDataAPI.SaveData(model);
                if (responseModel != null)
                {
                    string sensorValue = "";
                    for (int i = 0; i < responseModel.SensorList.Count; i++)
                    {
                        var item = responseModel.SensorList[i];
                        sensorValue += item.SensorId + ":" + item.Value + ":" + item.WarningState;
                        if (i < model.SensorList.Count - 1)
                            sensorValue += ";";
                    }
                    Clients.Group(channel).GetSensorData(model.DeviceId, model.State, sensorValue);
                }
            }
            catch
            {

            }
        }

        public void RemoteDevice(string channel, string deviceId, int state)
        {
            Clients.Group(channel).remote(deviceId, state);
        }

        public void Notification()
        {
            while (Business.Web.NotificationBusiness.notificationRemainingQueue.Count > 0)
            {
                var item = Business.Web.NotificationBusiness.notificationRemainingQueue.Dequeue();
                int newNotiAmount = notification.GetNumberOfUnreadNoticationByAccountId(item.AccountId);
                Clients.Group(item.Account.Channel).PushNotification(item.Title, newNotiAmount);
            }
        }

        public void CheckDevice()
        {
            DateTime Now = DateTime.UtcNow.AddHours(7);

            DateTime checkTime = Now.AddSeconds(-(int)Business.Dictionary.Time.CheckDeviceTime);

            List<Device> deviceList = deviceBusiness.GetDeviceListByActiveTimeAndState(checkTime, (int)Business.Dictionary.DeviceState.Offline);

            foreach (var item in deviceList)
            {
                item.State = (int)Business.Dictionary.DeviceState.Offline;
                deviceBusiness.UpdateDevice(item);
                Clients.Group(item.Account.Channel).GetSensorData(item.DeviceId, (int)Business.Dictionary.DeviceState.Offline, "");
                notification.SendWarningNotRespondNotification(item);
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (raspberryConnectionIdChannelDic.ContainsKey(Context.ConnectionId))
            {
                IAccountBusiness accountBusiness = new AccountBusiness();
                IDeviceBusiness deviceBusiness = new DeviceBusiness();
                ISensorBusiness sensorBusiness = new SensorBusiness();
                string channel = raspberryConnectionIdChannelDic[Context.ConnectionId];
                string accountId = accountBusiness.GetAccountByChannel(channel).AccountId;
                notification.SendWarningDisconnectionNotification(accountId);
                List<Business.Web.DeviceViewModel> deviceList = deviceBusiness.GetDeviceListByAccountId(accountId);
                SensorsDataViewModel model = new SensorsDataViewModel();
                foreach (var item in deviceList)
                {
                    model.DeviceId = item.DeviceId;
                    model.State = (int)Business.Dictionary.DeviceState.Offline;
                    sensorBusiness.SaveData(model);
                    Clients.Group(channel).GetSensorData(model.DeviceId, (int)Business.Dictionary.DeviceState.Offline, "");
                }

            }
            return base.OnDisconnected(stopCalled);
        }
    }
}