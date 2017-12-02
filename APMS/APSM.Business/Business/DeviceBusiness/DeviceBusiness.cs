using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APMS.DataAccess;

namespace APMS.Business.Web
{
    public class DeviceBusiness : IDeviceBusiness
    {
        private IRepository<Device> deviceRepository = new Repository<Device>();
        private IRepository<Sensor> sensorRepository = new Repository<Sensor>();
        private IRepository<SensorType> sensorTypeRepository = new Repository<SensorType>();
        public List<DeviceViewModel> GetDeviceListByAccountId(string accountId)
        {
            List<DeviceViewModel> model = new List<DeviceViewModel>();

            List<DataAccess.Sensor> sensorList = sensorRepository.GetAll().ToList();
            List<DataAccess.SensorType> sensorTypeList = sensorTypeRepository.GetAll().ToList();
            List<DataAccess.Device> deviceList = deviceRepository.GetAll().Where(x => x.AccountId.Equals(accountId.Trim())).ToList();

            foreach (var deviceItem in deviceList)
            {
                DeviceViewModel currentDevice = new DeviceViewModel(deviceItem);
                List<DataAccess.Sensor> SensorsOfCurrentDeviceList = sensorList.FindAll(x => x.DeviceId.Equals(deviceItem.DeviceId));
                foreach (var sensorItem in SensorsOfCurrentDeviceList)
                {
                    string sensorName = sensorTypeList.Find(x => x.SensorCode.Equals(sensorItem.SensorCode)).SensorName;
                    SensorViewModel currentSensor = new SensorViewModel(sensorItem, sensorName);
                    currentDevice.SensorList.Add(currentSensor);
                }
                model.Add(currentDevice);
            }

            return model;
        }

        public List<string> GetDeviceIdListByAccountId(string accountId)
        {
            List<string> result = deviceRepository.GetAll().Where(x => x.AccountId.Equals(accountId.Trim())).Select(x => x.DeviceId).ToList();

            return result;
        }

        public List<Device> GetDeviceListByActiveTimeAndState(DateTime activeTime, int state)
        {
            List<Device> result = deviceRepository.GetAll().Where(x => x.ActiveTime < activeTime && x.State != state).ToList();
            return result;
        }
        public void UpdateDevice(Device device)
        {
            deviceRepository.Update(device);
        }
    }
}
