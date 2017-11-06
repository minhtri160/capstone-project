using System;
using APMS.DataAccess;
using System.Linq;
using System.Text;

namespace APMS.Business.API
{
    public class LoginAPI : ILoginAPI
    {
        private char GetLetter()
        {
            string chars = "$%#@!abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ^&";
            Random rand = new Random();
            int num = rand.Next(0, chars.Length - 1);
            return chars[num];
        }

        public Account Login(LoginAPIViewModel model)
        {
            IRepository<Account> accRepository = new Repository<Account>();
            Account acc = accRepository.GetAll().Where(x => x.AccountId.Equals(model.Username) && x.Password.Equals(model.Password)).FirstOrDefault();

            if (acc == null)
                return null;

            StringBuilder tokenBuilder = new StringBuilder();
            for (int i = 0; i < 15; i++)
                tokenBuilder.Append(GetLetter());
            acc.Token = tokenBuilder.ToString();
            return acc;
        }
    }
}
