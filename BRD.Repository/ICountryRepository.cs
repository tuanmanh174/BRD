using BRD.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace BRD.Repository
{
    public interface ICountryRepository
    {
        IList<Countries> GetAll();
        Countries GetById(string account);
    }
}
