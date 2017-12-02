using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APMS.DataAccess;
using System.Security.Cryptography;
using System.Web;
using APMS.Business.API;

namespace APMS.Business.Web
{
    public class AccountBusiness : IAccountBusiness
    {
        private static string HashPassword(string password)
        {
            SHA1CryptoServiceProvider provider = new SHA1CryptoServiceProvider();
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] hashPassword = provider.ComputeHash(encoding.GetBytes(password));
            return Convert.ToBase64String(hashPassword);
        }

        private IRepository<Account> accountRepository = new Repository<Account>();

        private Account CheckUser(string username, string password)
        {
            Account result = accountRepository.GetAll().Where(x => x.AccountId.Equals(username) && x.Password.Equals(password)).FirstOrDefault();
            return result;
        }
        public Account CheckUserWeb(LoginWebViewModel model)
        {
            return CheckUser(model.Username, model.Password);
        }

        private char GetLetter(Random rand)
        {
            string chars = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int num = rand.Next(0, chars.Length - 1);
            return chars[num];
        }

        public Account CheckUserAPI(LoginAPIViewModel model)
        {
            Account acc = CheckUser(model.Username, model.Password);

            if (acc == null)
                return null;

            StringBuilder tokenBuilder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 15; i++)
                tokenBuilder.Append(GetLetter(random));
            acc.Token = tokenBuilder.ToString();
            accountRepository.Update(acc);
            return acc;
        }

        public Account CheckUserByToken(string token)
        {
            Account result = accountRepository.GetAll().Where(x => x.Token.Equals(token)).FirstOrDefault();
            return result;
        }

        public HttpCookie CreateCookie(string accountId)
        {
            HttpCookie cookie = new HttpCookie("Account");
            cookie["Username"] = accountId;
            Account result = accountRepository.GetAll().Where(x => x.AccountId.Equals(accountId)).FirstOrDefault();
            cookie["Password"] = HashPassword(result.Password);
            cookie.Expires = DateTime.Now.AddYears(1);
            return cookie;
        }

        public Account CheckUserByCookie(LoginViewModel model)
        {
            Account result = accountRepository.GetAll().Where(x => x.AccountId.Equals(model.Username)).FirstOrDefault();
            string hashPassword = HashPassword(result.Password);
            if (result!= null)
            {
                if (model.Password.Equals(hashPassword))
                    return result;
            }
            return null;
        }

        public Account GetAccountByChannel(string channel)
        {
            Account result = accountRepository.GetAll().Where(x => x.Channel.Equals(channel)).FirstOrDefault();
            return result;
        }
    }
}
