using Ad.Core;
using Ad.Core.Models;
using Ad.Core.Response;
using Ad.Core.Services;
using Ad.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Ad.Service
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionHistoryService _transactionHistoryRepository;
        private readonly ILogger<AccountService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(UserManager<ApplicationUser> userManager, ITransactionHistoryService transactionHistoryService, ILogger<AccountService> logger, IUnitOfWork unitOfWork)
        {
            _transactionHistoryRepository = transactionHistoryService;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<OperationResponse<ApplicationUser>> GetAccountByUserId(string id)
        {
            var getById =  await _unitOfWork.AccountRepository.SingleOrDefaultAsync(a => a.Id == id);
            if (getById == null)
                return new OperationResponse<ApplicationUser>("user does not exists!");

            return new OperationResponse<ApplicationUser>("user retrieved", getById, true);
        }

        public async Task<OperationResponse<UserProfile>> UpdateProfile(UserProfile userProfile)
        {
            if (await _unitOfWork.UserProfiles.AnyAsync(x => x.PhoneNumber == userProfile.PhoneNumber && x.ProfileId != userProfile.ProfileId && x.TenantId == userProfile.TenantId))
                return new OperationResponse<UserProfile>("Account with this phone number already exists!");

            var user = _userManager.Users.SingleOrDefault(x => x.Id == userProfile.ProfileId && x.TenantId == userProfile.TenantId);
            user.FirstName = userProfile.FirstName;
            user.LastName = userProfile.LastName;
            user.PhoneNumber = userProfile.PhoneNumber;
            await _userManager.UpdateAsync(user);
            await _unitOfWork.CommitChangesAsync();

            return new OperationResponse<UserProfile>("User profile updated successfully", userProfile, true);
        }

        public async Task<OperationResponse<bool>> TransferFunds(string senderAccountId, string recipientAccountId, decimal amount)
        {
            var senderAccount = await _unitOfWork.TransactionRepository.SingleOrDefaultAsync(a => a.Id == senderAccountId);
            var recipientAccount = await _unitOfWork.TransactionRepository.SingleOrDefaultAsync(a => a.Id == recipientAccountId);

            if (senderAccount == null || recipientAccount == null)
            {
                _logger.LogInformation("Transaction failed - Invalid sender or recipient.");
                return new OperationResponse<bool>("False");
            }

            if (!senderAccount.HasSufficientBalance(amount))
            {
                _logger.LogInformation("Transaction failed - Invalid sender or recipient.");
                return new OperationResponse<bool>("False");
            }

            senderAccount.Balance -= amount;
            recipientAccount.Balance += amount;
            _unitOfWork.CommitChanges();

            _transactionHistoryRepository.CreateTransaction(senderAccountId, recipientAccountId, amount);
             _unitOfWork.CommitChanges();
            _logger.LogInformation("Transaction successful - Sender: {Sender}, Recipient: {Recipient}, Amount: {Amount}", senderAccountId, recipientAccountId, amount);
            return new OperationResponse<bool>("True");
        }

      
    }
}
