using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using APMS.Business.API;
using APMS.Business.Web;

namespace APMS.Web.Controllers
{
    public class LoginController : ApiController
    {
        public HttpResponseMessage POST(LoginAPIViewModel model)
        {
            IAccountBusiness accountBusiness = new AccountBusiness();

            ResponseLoginAPIViewModel response = new ResponseLoginAPIViewModel();
            if (model != null)
            {
                APMS.DataAccess.Account acc = accountBusiness.CheckUserAPI(model);

                if (acc != null)
                {
                    IDeviceBusiness deviceBusiness = new DeviceBusiness();
                    response.Channel = acc.Channel;
                    response.DeviceIdList = deviceBusiness.GetDeviceIdListByAccountId(acc.AccountId);
                    response.Token = acc.Token;
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }
    }
}
