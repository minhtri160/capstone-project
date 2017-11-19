using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace APMS.Web
{
    public class SignalRHub : Hub
    {
        private static List<string> raspberryConnectionIdList = new List<string>();
        public async Task JoinGroup(string accountId, bool isRaspberry = false)
        {
            if (isRaspberry)
            {
                raspberryConnectionIdList.Add(Context.ConnectionId);
            }
            await Groups.Add(Context.ConnectionId, accountId);
        }

        public void PushSensorData(string accountId, string deviceId, int state, string sensorValue)
        {
            Clients.Group(accountId).GetSensorData(deviceId, state, sensorValue);
        }

        public void RemoteDevice(string accountId, string deviceId, int state)
        {
            Clients.Group(accountId).Remote(deviceId, state);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string raspberryConnectionId = raspberryConnectionIdList.Find(x => x.Equals(Context.ConnectionId));
            if (raspberryConnectionId!= null || !raspberryConnectionId.Trim().Equals(""))
            {
                //notification
            }
            return base.OnDisconnected(stopCalled);
        }
    }
}