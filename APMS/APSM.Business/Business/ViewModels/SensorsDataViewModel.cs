using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APMS.Business.Web
{
    public class SensorsDataViewModel
    {
        public SensorsDataViewModel()
        {
            SensorList = new List<SensorViewModel>();
        }

        public string DeviceId { get; set; }
        public int State { get; set; }
        public List<SensorViewModel> SensorList { get; set; }
    }

    public class SensorsWebInfoViewModel
    {
        public SensorsWebInfoViewModel()
        {
            SensorList = new List<SensorWebViewModel>();
        }

        public SensorsWebInfoViewModel(SensorsDataViewModel model)
        {
            SensorList = new List<SensorWebViewModel>();
            DeviceId = model.DeviceId;
            State = model.State;
        }


        public string DeviceId { get; set; }
        public int State { get; set; }
        public List<SensorWebViewModel> SensorList { get; set; }
    }
}
