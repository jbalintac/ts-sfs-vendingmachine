using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendingMachine.Models;
using VendingMachine.Services;

namespace VendingMachine.Controllers
{
    [Route("api/[controller]")]
    public class VendingMachineController : Controller
    {
        private VendingMachineContext _context;
        private CoinChangerService _coinChangerService;
        private VendingMachineService _vendingMachineService;

        public VendingMachineController(
            VendingMachineContext context,
            CoinChangerService coinChangerService,
            VendingMachineService vendingMachineService
            )
        {
            _context = context;
            _coinChangerService = coinChangerService;
            _vendingMachineService = vendingMachineService;

            vendingMachineService.InitializeData();
        }

        [HttpPost("InsertCoin")]
        public Session InsertCoin([FromBody]Coin coin)
        {
            return _vendingMachineService.InsertCoin(coin);
        }

        [HttpPost("ReturnCoins")]
        public TransactionViewModel ReturnCoins()
        {
            return _vendingMachineService.ReturnCoins();
        }


        [HttpPost("Purchase")]
        public TransactionViewModel Purchase([FromBody]Product product)
        {
            return _vendingMachineService.Purchase(product);
        }

        [HttpGet("Session")]
        public Session GetSession()
        {
            return _vendingMachineService.GetSession();
        }
    }
}
