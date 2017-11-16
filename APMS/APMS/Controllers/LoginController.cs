using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using APMS.Models;
using APMS.Business.API;

namespace APMS.Controllers
{
    public class LoginController : ApiController
    {
        public ResponseViewModel POST(LoginAPIViewModel model)
        {
            ILoginAPI loginBusiness = new LoginAPI();
            ResponseViewModel response = new ResponseViewModel();
            if (model != null)
            {
                APMS.DataAccess.Account acc = loginBusiness.Login(model);

                if (acc == null)
                {
                    response.HasErr = true;
                    response.ErrInfo = "Tài khoản hoặc mật khẩu không đúng!";
                }
                else
                {
                    response.ResponseObj = acc;
                }
            }
            else
            {
                response.HasErr = true;
                response.ErrInfo = "Model null!";
            }
            return response;
        }
    }
}
