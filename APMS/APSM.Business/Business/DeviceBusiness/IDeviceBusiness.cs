using System;
using System.Collections.Generic;
using APMS.DataAccess;

namespace APMS.Business.Web
{
    public interface IDeviceBusiness
    {
        List<DeviceViewModel> GetDeviceListByAccountId(string accountId);
        List<string> GetDeviceIdListByAccountId(string accountId);
        List<Device> GetDeviceListByActiveTimeAndState(DateTime activeTime, int state);
        bool UpdateDevice(Device device);
    }
}
