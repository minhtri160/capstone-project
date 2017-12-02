using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APMS.Business.Web
{

    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginWebViewModel : LoginViewModel
    {
        public string ErrMessage { get; set; }
    }
}
