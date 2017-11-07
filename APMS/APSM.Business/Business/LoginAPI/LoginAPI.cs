using System;
using APMS.DataAccess;
using System.Linq;
using System.Text;

namespace APMS.Business.API
{
    public class LoginAPI : ILoginAPI
    {
        private char GetLetter(Random rand)
        {
            string chars = "#@!abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ&";
            int num = rand.Next(chars.Length - 1);
            return chars[num];
        }

        public Account Login(LoginAPIViewModel model)
        {
            IRepository<Account> accountRepository = new Repository<Account>();
            Account acc = accountRepository.GetAll().Where(x => x.AccountId.Equals(model.Username) && x.Password.Equals(model.Password)).FirstOrDefault();

            if (acc == null)
                return null;

            if (acc.Token == null || acc.Token.Trim().Equals(""))
            {

                while (true) {
                    StringBuilder tokenBuilder = new StringBuilder();
                    Random random = new Random();
                    for (int i = 0; i < 15; i++)
                        tokenBuilder.Append(GetLetter(random));
                    string token = tokenBuilder.ToString();
                    if (accountRepository.GetAll().Where(x => x.Token.Equals(token)).FirstOrDefault() == null)
                    {
                        acc.Token = token;
                        break;
                    }
                }

            }
            acc.Status = 1;
            acc.ActiveTime = DateTime.Now;
            accountRepository.Update(acc);
            return acc;
        }
    }
}
