using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendingMachine.Models;

namespace VendingMachine.Services
{
    public class VendingMachineContext : DbContext
    {
        public VendingMachineContext(DbContextOptions<VendingMachineContext> options)
            : base(options)
        { }

        public DbSet<Session> Session { get; set; }
    }
}
