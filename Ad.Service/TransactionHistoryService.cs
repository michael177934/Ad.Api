using Ad.Core.Models;
using Ad.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ad.Service.TransactionHistoryService;

namespace Ad.Service
{
    public class TransactionHistoryService : ITransactionHistoryService
    {
        private List<Transaction> _transactions = new List<Transaction>();
        public Transaction CreateTransaction(string senderAccountId, string recipientAccountId, decimal amount)
        {
            var transaction = new Transaction
            {
                SenderAccountId = senderAccountId,
                RecipientAccountId = recipientAccountId,
                Amount = amount,
                Timestamp = DateTime.Now
            };

            _transactions.Add(transaction);

            return transaction;
        }

        public IEnumerable<Transaction> GetTransactionHistory()
        {
            return _transactions;
        }
    }
}
