using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendingMachine.Models;

namespace VendingMachine.Services
{
    public class VendingMachineService
    {

        private VendingMachineContext _context;
        private CoinChangerService _coinChangerService;
        private const string SUCCESS_MESSAGE = "Thank you";
        private const string PRODUCT_NOT_AVAILABLE = "Product not available.";
        private const string INSUFFICIENT_AMOUNT = "Insufficient amount";
        private const string INSUFFICIENT_CHANGE = "Insufficient coin to make a change.";

        public VendingMachineService(VendingMachineContext context, CoinChangerService coinChangerService)
        {
            _context = context;
            _coinChangerService = coinChangerService;
        }

        public void InitializeData()
        {
            if (!_context.Session.Any())
            {
                _context.Session.Add(new Session
                {
                    Id = 1,
                    AvailableProducts = new List<Product>
                    {
                        new Product { Id = 1, SessionId = 1, Name = "Tea", Price = 1.30M, Quantity = 10 },
                        new Product { Id = 2, SessionId = 1,  Name = "Espresso", Price = 1.80M, Quantity = 10 },
                        new Product { Id = 3, SessionId = 1,  Name = "Juice", Price = 1.80M, Quantity = 20 },
                        new Product { Id = 4, SessionId = 1,  Name = "Chicken soup", Price = 1.80M, Quantity = 15 }
                    },
                    AvailableCoins = new List<Coin>
                    {
                        new Coin { Id = 1, SessionId = 1,  Name = "10 cent", Value = 0.10M, Quantity = 100},
                        new Coin { Id = 2, SessionId = 1,  Name = "20 cent", Value = 0.20M, Quantity = 100},
                        new Coin { Id = 3, SessionId = 1,  Name = "50 cent", Value = 0.50M, Quantity = 100},
                        new Coin { Id = 4, SessionId = 1,  Name = "1 euro", Value = 1.0M, Quantity = 100}
                    },
                    CurrentInsertedCoin = 0
                });

                _context.SaveChanges();
            }
        }


        public Session InsertCoin(Coin coin)
        {
            var session = GetCurrentSession();

            var currentCoin = session.AvailableCoins.First(c => c.Id == coin.Id);
            currentCoin.Quantity++;

            session.CurrentInsertedCoin += currentCoin.Value;

            SaveSession(session);

            return session;
        }

        public TransactionViewModel ReturnCoins()
        {
            var session = GetCurrentSession();
            var lastCoinInserted = session.CurrentInsertedCoin;
            var returnCoined = _coinChangerService.CalculateChange(session.AvailableCoins, lastCoinInserted);

            var transactionMessage = lastCoinInserted > 0 ? 
                $"Returned: {lastCoinInserted}" : "";

            var coinMessage = lastCoinInserted > 0 ?
                $"Returned [{lastCoinInserted}]: " + string.Join(", ", returnCoined.Select(c => $"{c.Name} ({c.Quantity})")) 
                : "";


            session.CurrentInsertedCoin = 0;
            SaveSession(session);

            return new TransactionViewModel
            {
                Message = transactionMessage,
                CoinStatusMessage = coinMessage,
                Session = session
            };
        }

        public TransactionViewModel Purchase(Product product)
        {
            var changeReturn = new List<Coin>();
            var coinMessage = string.Empty;
            var session = GetCurrentSession();

            var productPurchasing = session.AvailableProducts.First(p => p.Id == product.Id);
            var change = session.CurrentInsertedCoin - productPurchasing.Price;

            // Check if product available
            if (session.AvailableProducts.First(p => p.Id == product.Id).Quantity <= 0)
            {
                return new TransactionViewModel
                {
                    Message = PRODUCT_NOT_AVAILABLE,
                    Session = session
                };
            }

            // Check if inserted amount sufficient
            if (change < 0)
            {
                return new TransactionViewModel
                {
                    Message = INSUFFICIENT_AMOUNT,
                    Session = session
                };
            }

            // Check if we can return a change
            try
            {
                if (change != 0)
                {
                    changeReturn = _coinChangerService
                        .CalculateChange(session.AvailableCoins, change);
                }
            }
            catch (ArgumentException ex) when (ex.Message == "Insufficient coin to make a change.")
            {
                return new TransactionViewModel
                {
                    Message = INSUFFICIENT_CHANGE,
                    Session = session
                };
            }

            // Create return message
            if (changeReturn.Any())
            {
                coinMessage = $"Returned [{session.CurrentInsertedCoin}]: " + string.Join(", ", changeReturn.Select(c => $"{c.Name} ({c.Quantity})"));
            }

            // Deduct values on product and inserted coins
            productPurchasing.Quantity--;
            session.CurrentInsertedCoin = 0;

            // Save transaction
            SaveSession(session);

            return new TransactionViewModel
            {
                Message = SUCCESS_MESSAGE,
                CoinStatusMessage = coinMessage,
                Session = session
            };
        }

        public Session GetSession()
        {
            return GetCurrentSession();
        }

        private void SaveSession(Session session)
        {
            _context.Session.Update(session);
            _context.SaveChanges();
        }

        private Session GetCurrentSession()
        {
            return _context.Session
                           .Include(s => s.AvailableCoins)
                           .Include(s => s.AvailableProducts)
                           .FirstOrDefault();
        }
    }
}
