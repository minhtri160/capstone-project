using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using APMS.Business.Web;
using System.Timers;

namespace APMS.Web
{
    public class SignalRHub : Hub
    {
        public Timer timer;

        public SignalRHub()
        {
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += OnTimeEvent;
        }

        private void OnTimeEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            Notification();
        }

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

        public void PushSensorData(string accountId, string deviceId, int state, string sensorValue)
        {
            int warningState = 0;
            Clients.Group(accountId).GetSensorData(deviceId, state, sensorValue, warningState);
        }

        public void RemoteDevice(string accountId, string deviceId, int state)
        {
            Clients.Group(accountId).Remote(deviceId, state);
        }

        public void Notification()
        {
            foreach(var item in APMS.Business.Web.Notification.notificationRemainingList)
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