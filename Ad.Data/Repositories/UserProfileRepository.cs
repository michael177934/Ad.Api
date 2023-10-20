using Ad.Core.Models;
using Ad.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ad.Data.Repositories
{
    public class UserProfileRepository : Repository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(DataContext context) : base(context)
        {
        }

        public DataContext DataContext => Context as DataContext;
    }
}
