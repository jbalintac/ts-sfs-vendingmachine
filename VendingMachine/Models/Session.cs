using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Models
{
    public class Session
    {
        public int Id { get; set; }
        public decimal CurrentInsertedCoin { get; set; }

        public List<Product> AvailableProducts { get; set; }
        public List<Coin> AvailableCoins { get; set; }
    }
}
