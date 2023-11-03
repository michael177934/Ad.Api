using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ad.Core.Response
{
    public class TransactionResponse
    {
        public string Id { get; set; }
        public string SenderAccountId { get; set; }
        public string RecipientAccountId { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsActive { get; set; }
        public bool HasSufficientBalance(decimal amount)
        {
            return Balance >= amount;
        }
    }
}
