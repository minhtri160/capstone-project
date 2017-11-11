using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using APMS.Models;
using APMS.Business.API;
using System.Web.Helpers;

namespace APMS.Controllers
{
    public class LoginController : ApiController
    {
        public HttpResponseMessage POST(LoginAPIViewModel model)
        {
            ILoginAPI loginBusiness = new LoginAPI();
            HttpResponseMessage responseMessage;
            if (model != null)
            {
                APMS.DataAccess.Account acc = loginBusiness.Login(model);

                if (acc == null)
                {
                    responseMessage = Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
                else
                {
                    responseMessage = Request.CreateResponse(HttpStatusCode.OK, new { access_token = acc.Token });
                }
            }
            else
            {
                responseMessage = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return responseMessage;
        }
    }
}
