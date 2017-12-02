using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APMS.Business.Web;

namespace APMS.Business.API
{
    public class LoginAPIViewModel : LoginViewModel
    {
    }

    public class ResponseLoginAPIViewModel
    {
        public ResponseLoginAPIViewModel()
        {
            DeviceIdList = new List<string>();
        }
        public string Channel { get; set; }
        public List<string> DeviceIdList { get; set; }
        public string Token { get; set; }
    }
}
