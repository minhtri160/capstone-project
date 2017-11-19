using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APMS.Business.API
{
    public class SaveSensorDataAPIViewModel
    {
        public SaveSensorDataAPIViewModel()
        {
            SensorParamList = new List<SensorParam>();
        }

        public string DeviceId { get; set; }
        public int State { get; set; }
        public List<SensorParam> SensorParamList { get; set; }
    }

    public class SensorParam
    {
        public string SensorId { get; set; }
        public double? Value { get; set; }
    }
}
