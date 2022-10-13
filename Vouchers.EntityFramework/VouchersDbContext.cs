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
using Vouchers.Files;
using Vouchers.Identities;
using Vouchers.Values;

namespace Vouchers.EntityFramework
{
    public sealed class VouchersDbContext : DbContext
    {
        public VouchersDbContext()
        {

        }

        public VouchersDbContext(DbContextOptions<VouchersDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new HolderTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new HolderTransactionRequestConfiguration());
            modelBuilder.ApplyConfiguration(new HolderTransactionItemConfiguration());
            modelBuilder.ApplyConfiguration(new IssuerTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new UnitConfiguration());
            modelBuilder.ApplyConfiguration(new AccountItemConfiguration());
            modelBuilder.ApplyConfiguration(new UnitTypeConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityConfiguration());
            
            modelBuilder.ApplyConfiguration(new IdentityConfiguration());
            modelBuilder.ApplyConfiguration(new LoginConfiguration());

            modelBuilder.ApplyConfiguration(new VoucherValueConfiguration());

            modelBuilder.ApplyConfiguration(new DomainConfiguration());
            modelBuilder.ApplyConfiguration(new DomainOfferConfiguration());
            modelBuilder.ApplyConfiguration(new DomainContractConfiguration());
            modelBuilder.ApplyConfiguration(new DomainOffersPerIdentityCounterConfiguration());
            modelBuilder.ApplyConfiguration(new DomainAccountConfiguration());

            modelBuilder.ApplyConfiguration(new CroppedImageConfiguration());

        }

        internal object Include()
        {
            throw new NotImplementedException();
        }

        #region Core

        public DbSet<Account> Accounts { get; set; }       
        public DbSet<AccountItem> AccountItems { get; set; }
        public DbSet<HolderTransaction> HolderTransactions { get; set; }
        public DbSet<HolderTransactionItem> HolderTransactionItems { get; set; }
        public DbSet<IssuerTransaction> IssuerTransactions { get; set; }
        public DbSet<HolderTransactionRequest> HolderTransactionRequests { get; set; }
        public DbSet<Unit> Units { get; set; }        
        public DbSet<UnitType> UnitTypes { get; set; }

        #endregion

        #region Values

        public DbSet<VoucherValue> VoucherValues { get; set; }

        #endregion

        #region Identities

        public DbSet<Identities.Identity> Identities { get; set; }
        public DbSet<Login> Logins { get; set; }

        #endregion

        #region Domains

        public DbSet<Domain> Domains { get; set; }

        public DbSet<DomainAccount> DomainAccounts { get; set; }
        public DbSet<DomainOffer> DomainOffers { get; set; }
        public DbSet<DomainContract> DomainContracts { get; set; }
        public DbSet<DomainOffersPerIdentityCounter> DomainOffersPerIdentityCounters { get; set; }
        #endregion

        #region Images
        public DbSet<CroppedImage> CroppedImages { get; set; }
        #endregion
    }
}
