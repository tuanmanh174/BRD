using System;
using System.Collections.Generic;
using System.Text;

namespace BRD.Repository
{
    public interface IUnitOfWork
    {
        IAccountRepository AccountRepository { get; }
        void Save();
    }
}
