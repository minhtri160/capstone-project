using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APMS.Business.Web
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            DeviceList = new List<Device>();
        }
        public List<Device> DeviceList { get; set; }
    }

    public class Device
    {
        public Device()
        {
            SensorList = new List<Sensor>();
        }
        public Device(DataAccess.Device device)
        {
            SensorList = new List<Sensor>();
            DeviceId = device.DeviceId;
            DeviceName = device.DeviceName;
            DeviceType = device.DeviceType;
            State = (int)device.State;
            Position = device.Position;
        }
        public List<Sensor> SensorList { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public int DeviceType { get; set; }
        public int State { get; set; }
        public string Position { get; set; }
    }

    public class Sensor
    {
        public Sensor()
        {

        }

        public Sensor(DataAccess.Sensor sensor, string sensorName)
        {
            SensorId = sensor.SensorId;
            SensorName = sensorName;
            Value = sensor.Value;
            Unit = sensor.Unit;
            WarningState = sensor.WarningState;
        } 
        public string SensorId { get; set; } 
        public string SensorName { get; set; }
        public double? Value { get; set; }
        public string Unit { get; set; }
        public int? WarningState { get; set; }
    }
}
