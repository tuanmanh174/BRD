using BRD.DataAccess;
using BRD.DataModel;
using BRD.Repository;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            var acc = _context.accounts.ToList();
            foreach (var item in acc)
            {
                var account = new DataModel.Account();
                account.AccountName = item.accountname;
                account.Id = item.id;
                ab.Add(account);
            }


            return ab;
        }

        public bool CheckConnection()
        {
            try
            {

                var a = _context.accounts.FirstOrDefault();


            }
            catch (SqlException)
            {
                return false;
            }
            return true;
        }

    }
}
