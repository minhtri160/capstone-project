using APMS.DataAccess;

namespace APMS.Business.API
{
    public interface ILoginAPI
    {
        Account Login(LoginAPIViewModel model);
    }
}
