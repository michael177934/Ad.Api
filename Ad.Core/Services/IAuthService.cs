using Ad.Core.Models;
using Ad.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ad.Core.Services
{
    public interface IAuthService
    {
        Task<OperationResponse<object>> Login(string email, string password, Guid tenantId);
        Task<OperationResponse<ApplicationUser>> Register(ApplicationUser user, string password, Guid tenantId);
    }
}
