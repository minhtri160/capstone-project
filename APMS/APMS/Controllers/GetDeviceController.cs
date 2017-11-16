using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using APMS.Business.API;
using APMS.DataAccess;

namespace APMS.Controllers
{
    public class GetDeviceController : ApiController
    {
        public GetDeviceAPIViewModel POST()
        {
            IGetDeviceAPI getDeviceAPI = new GetDevice();
            var header = this.Request.Headers;
            Account acc = new Account();
            if (header.Contains("token"))
            {
                string token = header.GetValues("token").First();
                acc = AuthorizeToken.Authorize(token);
            }
            GetDeviceAPIViewModel model = getDeviceAPI.Get(acc.AccountId);
            return model;
        }
    }
}
