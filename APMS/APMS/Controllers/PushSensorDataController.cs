﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using APMS.Business.Web;
using APMS.DataAccess;

namespace APMS.Controllers
{
    public class PushSensorDataController : ApiController
    {
        public HttpStatusCode POST(SaveSensorDataAPIViewModel model)
        {
            ISensorBusiness pushSensorData = new SensorBusiness();
            //var header = this.Request.Headers;
            //Account acc = new Account();
            //if (header.Contains("token"))
            //{
            //    string token = header.GetValues("token").First();
            //    acc = AuthorizeToken.Authorize(token);
            //}
            //bool result = pushSensorData.SaveData(model);
            //if (!result)
                return HttpStatusCode.BadRequest;
            return HttpStatusCode.OK;
        }
    }
}
