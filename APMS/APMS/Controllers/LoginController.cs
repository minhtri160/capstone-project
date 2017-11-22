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
        public HttpResponseMessage POST(LoginAPIViewModel model)
        {
            ILoginAPI loginBusiness = new LoginAPI();
            
            ResponseLoginAPIViewModel response = new ResponseLoginAPIViewModel();
            if (model != null)
            {
                APMS.DataAccess.Account acc = loginBusiness.Login(model);

                if (acc != null)
                {
                    IGetDeviceAPI getDevice = new GetDeviceAPI();
                    response.Channel = acc.Channel;
                    response.DeviceList = getDevice.Get(acc.AccountId).DeviceList;
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }
    }
}
