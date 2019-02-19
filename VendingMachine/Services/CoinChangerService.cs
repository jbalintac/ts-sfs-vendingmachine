using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendingMachine.Models;

namespace VendingMachine.Services
{
    public class CoinChangerService
    {
        public List<Coin> CalculateChange(List<Coin> coins, decimal amount)
        {
            var availableCoins = coins
                .Where(c => c.Quantity > 0)
                .ToList();

            var coinChange = new List<Coin>();

            while (amount > 0 && availableCoins.Any(c => c.Quantity > 0 && amount >= c.Value))
            {
                var coin = availableCoins
                            .OrderByDescending(c => c.Value)
                            .First(c => c.Quantity > 0 && amount >= c.Value);

                amount -= coin.Value;

                coinChange.Add(new Coin { Id = coin.Id, Value = coin.Value });
                coin.Quantity--;
            }

            var result = coinChange
                .GroupBy(
                    c => c.Id,
                    (b, g) => new Coin
                    {
                        Id = b,
                        Name = coins.FirstOrDefault(c => c.Id == b).Name,
                        Value = coins.FirstOrDefault(c => c.Id == b).Value,
                        Quantity = g.Count()
                    });

            if (amount > 0 || amount < 0)
            {
                var ex = new ArgumentException("Insufficient coin to make a change.");
                ex.Data.Add("CoinChange", result);

                throw ex;
            }

            return result.ToList();
        }
    }
}
