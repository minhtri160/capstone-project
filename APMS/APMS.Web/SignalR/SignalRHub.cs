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

namespace APMS.Web
{
    [HubName("signalRHub")]
    public class SignalRHub : Hub
    {
        //public Timer timer;
        //ISaveSensorDataAPI saveSensorDataAPI;

        //public SignalRHub()
        //{
        //    saveSensorDataAPI = new SaveSensorDataAPI();
        //    timer = new Timer();
        //    timer.Interval = 1000;
        //    timer.Elapsed += OnTimeEvent;
        //}

        //private void OnTimeEvent(Object source, System.Timers.ElapsedEventArgs e)
        //{
        //    Notification();
        //}

        private static Dictionary<string, string> raspberryConnectionIdAccountIdDic = new Dictionary<string, string>();
        public async Task JoinGroup(string accountId, bool isRaspberry = false)
        {
            if (isRaspberry)
            {
                try
                {
                    raspberryConnectionIdAccountIdDic.Add(Context.ConnectionId, accountId);
                }
                catch
                {

                }
            }
            await Groups.Add(Context.ConnectionId, accountId);
        }

        public void PushSensorData(string accountId, string Value)
        {

            string[] valueList = Value.Split(';');
            Business.API.SaveSensorDataAPIViewModel model = new SaveSensorDataAPIViewModel();
            model.DeviceId = valueList[0];
            model.State = Int32.Parse(valueList[1]);
            foreach (var item in valueList)
            {
                string[] sensorData = item.Split(':');
                Business.API.SensorParam sensor = new Business.API.SensorParam();
                sensor.SensorId = sensorData[0];
                sensor.Value = Double.Parse(sensorData[1]);
            }
            ISaveSensorDataAPI saveSensorDataAPI = new SaveSensorDataAPI();
            model = saveSensorDataAPI.SaveData(model);
            if (model != null)
            {
                string sensorValue = "";
                for (int i = 0; i < model.SensorParamList.Count; i++)
                {
                    var item = model.SensorParamList[i];
                    sensorValue += item.SensorId + ":" + item.Value + ":" + item.WarningState;
                    if (i < model.SensorParamList.Count)
                        sensorValue += ";";
                }
                Clients.Group(accountId).GetSensorData(model.DeviceId, model.State, sensorValue);
            }
        }

        public void RemoteDevice(string accountId, string deviceId, int state)
        {
            Clients.Group(accountId).remote(deviceId, state);
        }

        public void Notification()
        {
            foreach (var item in APMS.Business.Web.Notification.notificationRemainingList)
            {
                Clients.Group(item.AccountId).Notification(item.Title);
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (raspberryConnectionIdAccountIdDic.ContainsKey(Context.ConnectionId))
            {
                Business.Web.Notification.SendWarningDisconnectionNotification(raspberryConnectionIdAccountIdDic[Context.ConnectionId]);
            }
            return base.OnDisconnected(stopCalled);
        }
    }
}