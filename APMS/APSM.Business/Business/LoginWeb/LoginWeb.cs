using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APMS.DataAccess;

namespace APMS.Business.Web
{
    public class LoginWeb : ILoginWeb
    {
        IRepository<Account> accountRepository = new Repository<Account>();
        public Account CheckUser(LoginWebViewModel model)
        {
            Account result = accountRepository.GetAll().Where(x => x.AccountId.Equals(model.Username) && x.Password.Equals(model.Password)).FirstOrDefault();
            return result;
        }
    }
}
