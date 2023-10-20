using Ad.Core;
using Ad.Core.Repositories;
using Ad.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ad.Data
{
    public class UnitofWork : IUnitOfWork
    {
        private readonly DataContext _context;
        public UnitofWork(DataContext dataContext)
        {
            _context = dataContext;
            UserProfiles = new UserProfileRepository(_context);
            AccountRepository = new AccountRepository(_context);
            TransactionRepository = new TransactionRepository(_context);
        }
        public IUserProfileRepository UserProfiles { get; set; }
        public IAccountRepository AccountRepository { get; set; }
        public ITransactionRepository TransactionRepository { get; set; }

        public int CommitChanges()
        {
            return _context.SaveChanges();
        }

        public Task<int> CommitChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
