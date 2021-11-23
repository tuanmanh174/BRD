using BRD.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BRD.Repository
{
    public interface IAccountRepository
    {
        IList<Account> GetAll();
        Account GetById(string account);
        void Insert(Account account);
        void Update(Account account);
        void Delete(Account account);

    }
}
