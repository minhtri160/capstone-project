using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APMS.Business.API
{
    public class LoginAPIViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class ResponseLoginAPIViewModel
    {
        public ResponseLoginAPIViewModel()
        {
            DeviceList = new List<Device>();
        }
        public string Channel { get; set; }
        public List<Device> DeviceList { get; set; }
    }
}
