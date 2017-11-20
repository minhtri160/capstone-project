using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APMS.Business.Web
{
    public interface IHomeWeb
    {
        HomeViewModel GetDeviceListByAccountId(string accountId);
    }
}
