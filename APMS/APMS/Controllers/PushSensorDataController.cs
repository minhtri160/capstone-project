using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using APMS.Business.API;
using APMS.DataAccess;
using APMS.Models;
using Newtonsoft.Json;

namespace APMS.Controllers
{
    public class PushSensorDataController : ApiController
    {
        public HttpResponseMessage POST(RequestViewModel model)
        {

            IPushSensorDataAPI pushSensorData = new PushSensorDataAPI();
            HttpResponseMessage response;
            try
            {
                string token = model.Token;

                if (token != null)
                {
                    //string token = header.GetValues("token").First();
                    Account acc = AuthorizeToken.Authorize(token);
                    if (acc != null)
                    {

                        PushSensorDataAPIViewModel sensorDataAPIViewModel = JsonConvert.DeserializeObject<PushSensorDataAPIViewModel>(model.RequestObject.ToString());
                        bool result = pushSensorData.SaveData(sensorDataAPIViewModel);
                        if (!result)
                            response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        response = new HttpResponseMessage(HttpStatusCode.OK);

                    }
                    else
                    {
                        response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    }

                }
                else
                {
                    response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
            }
            catch
            {
                response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            //var header = this.Request.Headers;
            //Account acc = new Account();
            //if (header.Contains("token"))
            //{
            //    string token = header.GetValues("token").First();
            //    acc = AuthorizeToken.Authorize(token);
            //}
            return response;
        }
    }
}
