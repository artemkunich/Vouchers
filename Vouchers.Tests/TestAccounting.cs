using System;
using System.Linq;
using Xunit;
using Vouchers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using A = Vouchers.Accounting;
using V = Vouchers.Values;
using U = Vouchers.Users;

using AI = Vouchers.Infrastructure.EF.Accounting;
using VI = Vouchers.Infrastructure.EF.Values;
using UI = Vouchers.Infrastructure.EF.Users;

using Vouchers.Infrastructure;
using Vouchers.Infrastructure.EF;
using Vouchers.Application;
using Vouchers.Application.DTO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Vouchers.Tests
{
    public class TestAccounting
    {
        private IServiceProvider serviceProvider;
        private IServiceCollection services;

        private AdminService usersService;
        private ValuesService valuesService;
        private AccountingService accountingService;
        private DTOFactory dtoFactory;

        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(
            builder => {builder.AddConsole(); }
        );

        public TestAccounting()
        {
            services = new ServiceCollection();
            
            services.AddTransient<A.IAccounts, AI.Repositories.Accounts>();
            services.AddTransient<A.IIssuerTransactions, AI.Repositories.IssuerTransactions>();
            services.AddTransient<A.IHolderTransactions, AI.Repositories.HolderTransactions>();
            services.AddTransient<A.IUserAccounts, AI.Repositories.UserAccounts>();
            services.AddTransient<A.IVouchers, AI.Repositories.Vouchers>();
            services.AddTransient<A.IVoucherValues, AI.Repositories.VoucherValues>();

            services.AddTransient<U.IUserAccounts, UI.Repositories.UserAccounts>();
            services.AddTransient<U.IHashCalculator, RNGHashCalculator>();


            services.AddTransient<V.IUserAccounts, VI.Repositories.UserAccounts>();
            services.AddTransient<V.IVoucherValues, VI.Repositories.VoucherValues>();

            services.AddTransient<A.TransactionFactory>();
            services.AddTransient<A.TransactionFactory>();

            services.AddTransient<U.UserAccountFactory>();

            services.AddTransient<V.VoucherValueFactory>();
            services.AddTransient<V.VoucherFactory>();

            
            services.AddTransient<AdminService>();
            services.AddTransient<ValuesService>();
            services.AddTransient<AccountingService>();
            services.AddTransient<DTOFactory>();


            services.AddDbContext<VouchersContext>(options => options.UseSqlServer("server=T480SAKU\\SQLEXPRESS;database=Vouchers;trusted_connection=true;").EnableSensitiveDataLogging().UseLoggerFactory(loggerFactory));
            serviceProvider = services.BuildServiceProvider();

            //services.AddDbContext<VouchersContext>(options => options.UseInMemoryDatabase("InMemoryDbForTesting").UseInternalServiceProvider(serviceProvider));
            //serviceProvider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

            usersService = serviceProvider.GetRequiredService<AdminService>();
            valuesService = serviceProvider.GetRequiredService<ValuesService>();
            accountingService = serviceProvider.GetRequiredService<AccountingService>();

            dtoFactory = serviceProvider.GetRequiredService<DTOFactory>();

        }

        [Fact]
        public void Test1()
        {
            //usersService.RemoveUserAccount("ArtemKunich");
            var admin = usersService.GetUserAccount("Admin", "Admin");

            var permissionsDTO = dtoFactory.CreatePermissionsDTO(true, true, false);
            usersService.CreateUserAccount("ArtemKunich", "artem@gmail.com", "artem", permissionsDTO, admin);

            permissionsDTO = dtoFactory.CreatePermissionsDTO(false, true, false);
            usersService.CreateUserAccount("EkaterinaKunich", "ekaterina@gmail.com", "ekaterina", permissionsDTO, admin);

            var issuer = usersService.GetUserAccount("ArtemKunich", "artem");
            Assert.True(issuer != null);

            var value = valuesService.CreateVoucherValue(dtoFactory.CreateVoucherValueDTO("Test", "Massage", "1 minute of massage", "ArtemKunich"), issuer);
            var voucher = valuesService.AddVoucher(value.Id, dtoFactory.CreateVoucherDTO(DateTime.Today, DateTime.MaxValue, true), issuer);

            accountingService.CreateIssuerTransaction(accountingService.GetVoucherQuantity(100, voucher.Id), issuer);
            
            var transactionDto = dtoFactory.CreateTransactionDTO("ArtemKunich", "EkaterinaKunich", accountingService.GetVoucherValueQuantity(30, value.Id));
            transactionDto.AddItem(accountingService.GetVoucherQuantity(30, voucher.Id));

            accountingService.CreateHolderTransaction(transactionDto, issuer);

        }
    }
}
