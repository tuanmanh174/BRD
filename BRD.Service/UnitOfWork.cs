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
        private IAccountRepository _accountRepository;

        public UnitOfWork(Context context)
        {
            _context = context;
        }

        public IAccountRepository AccountRepository
        {
            get
            {
                return _accountRepository = _accountRepository ?? new AccountService(_context);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
