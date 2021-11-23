using BRD.DataAccess;
using BRD.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace BRD.Service
{
    public class UnitOfWork : IUnitOfWork
    {
        private Context _context;
        private ICountryRepository _countriesRepository;

        public UnitOfWork(Context context)
        {
            _context = context;
        }

   

        public ICountryRepository CountriesRepository
        {
            get
            {
                return _countriesRepository = _countriesRepository ?? new CountryService(_context);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
