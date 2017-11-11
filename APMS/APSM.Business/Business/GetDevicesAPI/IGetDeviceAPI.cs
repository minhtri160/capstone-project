using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APMS.Business.API
{
    public interface IGetDeviceAPI
    {
        GetDeviceAPIViewModel Get(string accountId);
    }
}
