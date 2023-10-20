using Ad.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ad.Core.Services
{
    public interface ITransactionHistoryService
    {
        Transaction CreateTransaction(string senderAccountId, string recipientAccountId, decimal amount);
        IEnumerable<Transaction> GetTransactionHistory();
    }
}
