using System.Collections.Generic;
using System.Linq;
using APMS.DataAccess;

namespace APMS.Business.API
{
    public class GetDevice : IGetDeviceAPI
    {
        public GetDeviceAPIViewModel Get(string accountId)
        {
            IRepository<APMS.DataAccess.Sensor> sensorRepository = new Repository<APMS.DataAccess.Sensor>();
            IRepository<APMS.DataAccess.Device> deviceRepository = new Repository<APMS.DataAccess.Device>();
            IRepository<APMS.DataAccess.Rule> ruleRepository = new Repository<APMS.DataAccess.Rule>();
            GetDeviceAPIViewModel model = new GetDeviceAPIViewModel();
            List<APMS.DataAccess.Device> deviceList = deviceRepository.GetAll().Where(x => x.AccountId.Equals(accountId.Trim())).ToList();
            foreach (var item in deviceList)
            {
                Device device = new Device();
                device.deviceId = item.DeviceId;
                List<APMS.DataAccess.Sensor> sensorList = sensorRepository.GetAll().Where(x => x.DeviceId.Equals(item.DeviceId)).ToList();
                foreach (var item2 in sensorList)
                {
                    Sensor sensor = new Sensor();
                    sensor.SensorId = item2.SensorId;
                    List<APMS.DataAccess.Rule> ruleList = ruleRepository.GetAll().Where(x => x.SensorId.Equals(item2.SensorId)).ToList();
                    foreach (var item3 in ruleList)
                    {
                        Rule rule = new Rule(item3);
                        sensor.RuleList.Add(rule);
                    }
                    device.SensorList.Add(sensor);
                }
                model.DeviceList.Add(device);
            }

            return model;
        }
    }
}
