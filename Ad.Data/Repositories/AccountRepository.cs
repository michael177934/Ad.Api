using Ad.Core.Models;
using Ad.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ad.Data.Repositories
{
    public class AccountRepository : Repository<ApplicationUser>, IAccountRepository
    {
        public AccountRepository(DataContext context) : base(context)
        {
        }

        public DataContext DataContext => Context as DataContext;
    }
}
