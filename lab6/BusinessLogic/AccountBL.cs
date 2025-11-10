using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class AccountBL
    {
        AccountDA accountDA = new AccountDA();

        public List<Account> GetAll()
        {
            return accountDA.GetAll();
        }

        public Account GetByID(string accountName)
        {
            return accountDA.GetByID(accountName);
        }

        public List<Account> Find(string key)
        {
            List<Account> list = GetAll();
            List<Account> result = new List<Account>();

            foreach (var item in list)
            {
                if (item.AccountName.Contains(key) ||
                    item.FullName.Contains(key) ||
                    item.Email.Contains(key))
                    result.Add(item);
            }
            return result;
        }

        public int Insert(Account account)
        {
            return accountDA.Insert(account);
        }

        public int Update(Account account)
        {
            return accountDA.Update(account);
        }

        public int Delete(string accountName)
        {
            return accountDA.Delete(accountName);
        }

        public bool Login(string username, string password)
        {
            var account = GetByID(username);
            return account != null && account.Password == password;
        }
    }
}
