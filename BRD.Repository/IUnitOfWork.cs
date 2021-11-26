using System;
using System.Collections.Generic;
using System.Text;

namespace BRD.Repository
{
    public interface IUnitOfWork
    {
        ICountryRepository CountriesRepository { get; }
        IAccountRepository AccountRepository { get; }
        void Save();
    }
}
