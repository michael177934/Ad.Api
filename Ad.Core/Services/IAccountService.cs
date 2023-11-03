using Ad.Core.Models;
using Ad.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ad.Core.Services
{
    public interface IAccountService
    {
        Task<OperationResponse<ApplicationUser>> GetAccountByUserId(string id);
        Task<OperationResponse<ApplicationUser>> UpdateProfile(ApplicationUser userProfile);
        Task<OperationResponse<bool>> TransferFunds(string senderAccountId, string recipientAccountId, decimal amount);
        Task<OperationResponse<bool>> TransferFunds(Transaction transaction);
    }
}

