using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Models
{
    public class TransactionViewModel
    {
        public Session Session { get; set; }
        public string Message { get; set; }
        public string CoinStatusMessage { get; set; }
    }
}
