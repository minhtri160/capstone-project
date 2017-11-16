using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APMS.Models
{
    public class ResponseViewModel
    {
        public ResponseViewModel()
        {
            HasErr = false;
            ErrInfo = "";
        }

        public ResponseViewModel(object responseObj)
        {
            HasErr = false;
            ErrInfo = "";
            ResponseObj = responseObj;
        }

        public bool HasErr { get; set; }
        public string ErrInfo { get; set; }
        public object ResponseObj { get; set; }
    }
}