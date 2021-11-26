using BRD.DataAccess;
using BRD.DataModel;
using BRD.Repository;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
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

        public IList<DataModel.Account> GetAll()
        {
            var ab = new List<DataModel.Account>();
            var acc = _context.ACCOUNT.ToList();
            foreach (var item in acc)
            {
                var account = new DataModel.Account();
                account.AccountName = item.ACCOUNTNAME;
                account.Id = item.ID;
                ab.Add(account);
            }

            return ab;
        }

    }
}
