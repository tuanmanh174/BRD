using BRD.DataAccess;
using BRD.DataModel;
using BRD.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BRD.Service
{
    public class AccountService : IAccountRepository
    {
        private Context _context;

        public AccountService(Context context)
        {
            _context = context;
        }

        public IList<Account> GetAll()
        {
            return _context.Accounts.ToList();
        }
        public Account GetById(string account)
        {
            var _account = _context.Accounts.Where(x => x.Name == account).FirstOrDefault();
            return _account;
        }
        public void Insert(Account account)
        {
            _context.Accounts.Add(new Account());
        }
        public void Update(Account account)
        {
            _context.Accounts.Update(account);
        }
        public void Delete(Account account)
        {
            _context.Accounts.Remove(account);
        }

    }
}
