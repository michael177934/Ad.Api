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
using System.Security.Principal;
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
        //public async Task<OperationResponse<ApplicationUser>> GetAccountByUserId(string id)
        //{
        //    try
        //    {
        //        var getById = await _unitOfWork.AccountRepository.SingleOrDefaultAsync(a => a.Id == id);
        //        if (getById == null)
        //            return new OperationResponse<ApplicationUser>("user does not exists!");
        //        _logger.LogInformation("user does not exists!", id);

        //        _logger.LogInformation("user retrived!", id);
        //        return new OperationResponse<ApplicationUser>("user retrieved", getById, true);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }


        //}

        public async Task<OperationResponse<ApplicationUser>> GetAccountByUserId(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return new OperationResponse<ApplicationUser>("Invalid user ID.");
                }

                var getById = await _unitOfWork.AccountRepository.SingleOrDefaultAsync(a => a.Id == id);
                if (getById == null)
                {
                    _logger.LogInformation($"User with ID '{id}' does not exist.");
                    return new OperationResponse<ApplicationUser>($"User with ID '{id}' does not exist.");
                }

                _logger.LogInformation($"User with ID '{id}' retrieved.");
                return new OperationResponse<ApplicationUser>($"User with ID '{id}' retrieved", getById, true);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while retrieving the user.", ex);
                throw;
            }
        }



        public async Task<OperationResponse<ApplicationUser>> UpdateProfile(ApplicationUser userProfile)
        {
            if (await _unitOfWork.UserProfiles.AnyAsync(x => x.PhoneNumber == userProfile.PhoneNumber && x.TenantId == userProfile.TenantId))
                return new OperationResponse<ApplicationUser>("Account with this phone number already exists!");

            var user = await _unitOfWork.AccountRepository.SingleOrDefaultAsync(x => x.Id == userProfile.Id && x.TenantId == userProfile.TenantId);
            user.FirstName = userProfile.FirstName;
            user.LastName = userProfile.LastName;
            user.PhoneNumber = userProfile.PhoneNumber;
            await _userManager.UpdateAsync(user);
            await _unitOfWork.CommitChangesAsync();

            return new OperationResponse<ApplicationUser>("User profile updated successfully", userProfile, true);
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
                _logger.LogInformation("Transaction failed -  sender Has no sufficient balance.");
                return new OperationResponse<bool>("False");
            }

            senderAccount.Balance -= amount;
            recipientAccount.Balance += amount;
            _unitOfWork.CommitChanges();

            _transactionHistoryRepository.CreateTransaction(senderAccountId, recipientAccountId, amount);
            _unitOfWork.CommitChanges();
            _logger.LogInformation($"Transaction successful - Sender: {senderAccountId}, Recipient: {recipientAccountId}, Amount: {amount}", senderAccountId, recipientAccountId, amount);
            return new OperationResponse<bool>("True");
        }



        public async Task<OperationResponse<bool>> TransferFunds(Transaction transaction)
        {
            var senderAccount = await _unitOfWork.AccountRepository.SingleOrDefaultAsync(a => a.AccountNumber == transaction.SenderAccountId);
            var recipientAccount = await _unitOfWork.AccountRepository.SingleOrDefaultAsync(a => a.AccountNumber == transaction.RecipientAccountId);

            if (senderAccount == null || recipientAccount == null)
            {
                _logger.LogInformation("Transaction failed - Invalid sender or recipient.");
                return new OperationResponse<bool>("False");
            }

            if (!senderAccount.HasSufficientBalance(transaction.Amount))
            {
                _logger.LogInformation("Transaction failed - Sender has insufficient balance.");
                return new OperationResponse<bool>("False");
            }
            else
            {
                senderAccount.Balance -= transaction.Amount;
                recipientAccount.Balance += transaction.Amount;

              var creditTransaction =  _transactionHistoryRepository.CreateTransaction(transaction.SenderAccountId, transaction.RecipientAccountId, transaction.Amount);
              await _unitOfWork.TransactionRepository.AddAsync(creditTransaction);
               await _unitOfWork.CommitChangesAsync();

                _logger.LogInformation($"Transaction successful - Sender: {transaction.SenderAccountId}, Recipient: {transaction.RecipientAccountId}, Amount: {transaction.Amount}");
                return new OperationResponse<bool>("Transaction successful",true);

            }
        }


        private bool IsEligibleToReceiveFunds(decimal transferAmount, Transaction recipientAccount)
        {
            if (!recipientAccount.IsActive)
            {
                return false;
            }
            decimal maxBalanceLimit = 1000000;
            if (recipientAccount.Balance + transferAmount > maxBalanceLimit)
            {
                return false;
            }
            return true;
        }



    }
}
