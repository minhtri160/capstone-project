using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace APMS.Web.SignalR
{
    public class SignalRHub : Hub
    {
        public async Task JoinGroup(string accountId)
        {
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

        
    }
}