
using System.Collections.Generic;

namespace APMS.Business.API
{
    public class GetDeviceAPIViewModel
    {
        public GetDeviceAPIViewModel()
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

        public string deviceId { get; set; }
        public List<Sensor> SensorList { get; set; }
    }

    public class Sensor
    {
        public Sensor()
        {
            RuleList = new List<Rule>();
        }
        public string SensorId { get; set; }
        public List<Rule> RuleList { get; set; }
    }

    public class Rule
    {
        public Rule()
        {

        }
        public Rule(APMS.DataAccess.Rule model)
        {
            State = model.State;
            OperatorType = model.OperatorType;
            Value = model.Value;
        }
        public int State { get; set; }
        public int OperatorType { get; set; }
        public double? Value { get; set; }
    }
}
