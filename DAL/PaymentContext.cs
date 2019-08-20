using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using EcheancierDotNet.Models;

namespace EcheancierDotNet.DAL
{
    public class PaymentContext : DbContext
    {

        public PaymentContext() : base("PaymentContext")    // We can pass here the connection string if needed
        {

        }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<BankAccount> BankAccount { get; set; }
        public DbSet<Flow> Flow { get; set; }
        public DbSet<Upload> Upload { get; set; }
        public DbSet<TurboSupplier> TurboSupplier { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}