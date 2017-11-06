using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APMS.DataAccess;

namespace APMS.Business.Web
{
    public interface ILoginWeb
    {
        Account CheckUser(LoginWebViewModel model);
    }
}
