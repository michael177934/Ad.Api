using Ad.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ad.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IUserProfileRepository UserProfiles { get; set; }
        IAccountRepository AccountRepository { get; set; }
        ITransactionRepository TransactionRepository { get; set; }
        Task<int> CommitChangesAsync();
        int CommitChanges();
    }
}
