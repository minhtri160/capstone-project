using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using APMS.Business.API;
using APMS.DataAccess;
using APMS.Models;

namespace APMS.Controllers
{
    public class GetDeviceController : ApiController
    {
        public HttpResponseMessage POST(RequestViewModel model)
        {
            IGetDeviceAPI getDeviceAPI = new GetDevice();
            Account acc = new Account();
            HttpResponseMessage response;
            string token = model.Token;
            if (token != null)
            {
                //string token = header.GetValues("token").First();
                acc = AuthorizeToken.Authorize(token);
                if (acc != null)
                {
                    GetDeviceAPIViewModel responseModel = getDeviceAPI.Get(acc.AccountId);
                    response = Request.CreateResponse(HttpStatusCode.OK, responseModel);
                    return response;
                }

            }
            response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            return response;
        }
    }
}
