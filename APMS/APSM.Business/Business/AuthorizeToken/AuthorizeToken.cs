using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APMS.DataAccess;

namespace APMS.Business.API
{
    public static class AuthorizeToken
    {
        public static Account Authorize(string token)
        {
            IRepository<Account> repository = new Repository<Account>();
            Account result = repository.GetAll().Where(x => x.Token.Trim().Equals(token.Trim())).FirstOrDefault();
            return result;
        }
    }
}
