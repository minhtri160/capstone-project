using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APMS.DataAccess;

namespace APMS.Business.Web
{
    public class HomeWeb: IHomeWeb
    {
        public HomeViewModel GetDeviceListByAccountId(string accountId)
        {
            IRepository<DataAccess.Device> deviceRepository = new Repository<DataAccess.Device>();
            IRepository<DataAccess.Sensor> sensorRepository = new Repository<DataAccess.Sensor>();
            IRepository<DataAccess.SensorType> sensorTypeRepository = new Repository<DataAccess.SensorType>();
            HomeViewModel model = new HomeViewModel();

            List<DataAccess.Sensor> sensorList = sensorRepository.GetAll().ToList();
            List<DataAccess.SensorType> sensorTypeList = sensorTypeRepository.GetAll().ToList();
            List<DataAccess.Device> deviceList = deviceRepository.GetAll().Where(x => x.AccountId.Equals(accountId.Trim())).ToList();

            foreach (var deviceItem in deviceList)
            {
                Device currentDevice = new Device(deviceItem);
                List<DataAccess.Sensor> SensorsOfCurrentDeviceList = sensorList.FindAll(x => x.DeviceId.Equals(deviceItem.DeviceId));
                foreach (var sensorItem in SensorsOfCurrentDeviceList)
                {
                    string sensorName = sensorTypeList.Find(x => x.SensorCode.Equals(sensorItem.SensorCode)).SensorName;
                    Sensor currentSensor = new Sensor(sensorItem, sensorName);
                    currentDevice.SensorList.Add(currentSensor);
                }
                model.DeviceList.Add(currentDevice);
            }

            return model;
        }
    }
}
