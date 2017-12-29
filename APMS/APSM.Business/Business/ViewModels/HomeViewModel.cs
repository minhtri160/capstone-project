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
            DeviceList = new List<DeviceViewModel>();
        }
        public List<DeviceViewModel> DeviceList { get; set; }
        public int NewNotificationAmount { get; set; }
    }

    public class DeviceViewModel
    {
        public DeviceViewModel()
        {
            SensorList = new List<SensorViewModel>();
        }
        public DeviceViewModel(DataAccess.Device device)
        {
            SensorList = new List<SensorViewModel>();
            DeviceId = device.DeviceId;
            DeviceName = device.DeviceName;
            DeviceType = device.DeviceType;
            State = (int)device.State;
            Position = device.Position;
        }
        public List<SensorViewModel> SensorList { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public int DeviceType { get; set; }
        public int State { get; set; }
        public string Position { get; set; }
    }

    public class SensorViewModel
    {
        public SensorViewModel()
        {

        }

        public SensorViewModel(DataAccess.Sensor sensor, string sensorName)
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
    public class SensorWebViewModel
    {
        public SensorWebViewModel()
        {

        }

        public SensorWebViewModel(SensorViewModel sensor)
        {
            SensorId = sensor.SensorId;
            SensorName = sensor.SensorName;
            Unit = sensor.Unit;
            WarningState = sensor.WarningState;
        }
        public string SensorId { get; set; }
        public string SensorName { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public int? WarningState { get; set; }
    }
}
