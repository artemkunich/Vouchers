using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vouchers.Core;
using Vouchers.Domains;
using Vouchers.EntityFramework.Configurations;
using Vouchers.Identities;
using Vouchers.Values;

namespace Vouchers.EntityFramework
{
    public class VouchersDbContext : DbContext
    {
        public VouchersDbContext()
        {

        }

        public VouchersDbContext(DbContextOptions<VouchersDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.ApplyConfiguration(new DomainConfiguration());
            modelBuilder.ApplyConfiguration(new DomainAccountConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityDetailConfiguration());
            modelBuilder.ApplyConfiguration(new HolderTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new HolderTransactionItemConfiguration());
            modelBuilder.ApplyConfiguration(new IssuerTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new VoucherConfiguration());
            modelBuilder.ApplyConfiguration(new VoucherAccountConfiguration());
            modelBuilder.ApplyConfiguration(new VoucherValueConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityConfiguration());
            
            modelBuilder.ApplyConfiguration(new IdentityDetailConfiguration());
            modelBuilder.ApplyConfiguration(new LoginConfiguration());

            modelBuilder.ApplyConfiguration(new VoucherValueDetailConfiguration());

            modelBuilder.ApplyConfiguration(new DomainDetailConfiguration());
            modelBuilder.ApplyConfiguration(new DomainOfferConfiguration());
            modelBuilder.ApplyConfiguration(new DomainContractConfiguration());
        }

        internal object Include()
        {
            throw new NotImplementedException();
        }

        #region Core

        public DbSet<Domain> Domains { get; set; }
        public DbSet<DomainAccount> DomainAccounts { get; set; }       
        public DbSet<VoucherAccount> VoucherAccounts { get; set; }
        public DbSet<HolderTransaction> HolderTransactions { get; set; }
        public DbSet<HolderTransactionItem> HolderTransactionItems { get; set; }
        public DbSet<IssuerTransaction> IssuerTransactions { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }        
        public DbSet<VoucherValue> VoucherValues { get; set; }
        public DbSet<Identity> Identities { get; set; }

        #endregion

        #region Values

        public DbSet<VoucherValueDetail> VoucherValueDetails { get; set; }

        #endregion

        #region Identities

        public DbSet<IdentityDetail> IdentityDetails { get; set; }
        public DbSet<Login> Logins { get; set; }

        #endregion

        #region DomainDetails

        public DbSet<DomainDetail> DomainDetails { get; set; }
        public DbSet<DomainOffer> DomainOffers { get; set; }
        public DbSet<DomainContract> DomainContracts { get; set; }
        #endregion
    }
}
