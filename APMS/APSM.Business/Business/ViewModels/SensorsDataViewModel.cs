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

    
}
