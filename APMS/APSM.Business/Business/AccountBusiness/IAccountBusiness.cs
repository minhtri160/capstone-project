using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APMS.DataAccess;
using System.Web;
using APMS.Business.API;

namespace APMS.Business.Web
{
    public interface IAccountBusiness
    {
        Account CheckUserWeb(LoginWebViewModel model);
        Account CheckUserAPI(LoginAPIViewModel model);
        Account CheckUserByToken(string token);
        Account CheckUserByCookie(LoginViewModel model);
        HttpCookie CreateCookie(string accountId);
        Account GetAccountByChannel(string channel);
    }
}
