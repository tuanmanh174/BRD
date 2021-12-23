using BRD.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BRD.Repository
{
    public interface IAccountRepository
    {
        IList<Account> GetAll();
        bool CheckConnection();
    }
}
