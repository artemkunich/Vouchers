using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using A = Vouchers.Accounting;
using V = Vouchers.Values;
using U = Vouchers.Auth;
using R = Vouchers.Views;
using M = Vouchers.MF;

using AI = Vouchers.Infrastructure.EF.Accounting;
using VI = Vouchers.Infrastructure.EF.Values;
using UI = Vouchers.Infrastructure.EF.Users;
using RI = Vouchers.Infrastructure.EF.Reports;
using MI = Vouchers.Infrastructure.EF.MF;

using Vouchers.Infrastructure;
using Vouchers.Infrastructure.EF;
using Vouchers.Application;
using Microsoft.EntityFrameworkCore;
using Vouchers.WPF.Views;
using Vouchers.WPF.ViewModel;

namespace Vouchers.WPF.Mono
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private IServiceProvider serviceProvider;
        private IServiceCollection services;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            services = new ServiceCollection();

            services.AddTransient<A.IAccounts, AI.Repositories.Accounts>();
            services.AddTransient<A.IIssuerTransactions, AI.Repositories.IssuerTransactions>();
            services.AddTransient<A.IHolderTransactions, AI.Repositories.HolderTransactions>();
            services.AddTransient<A.IUserAccounts, AI.Repositories.UserAccounts>();
            services.AddTransient<A.IVouchers, AI.Repositories.Vouchers>();
            services.AddTransient<A.IVoucherValues, AI.Repositories.VoucherValues>();
            services.AddTransient<A.ICollectionRequests, AI.Repositories.CollectionRequests>();

            services.AddTransient<U.IUserCredentialsRepository, UI.Repositories.UserCredentialsRepository>();
            services.AddTransient<U.IHashCalculator, RNGHashCalculator>();


            services.AddTransient<V.IUserAccounts, VI.Repositories.UserAccounts>();
            services.AddTransient<V.IVoucherValueDetails, VI.Repositories.VoucherValues>();

            services.AddTransient<R.IVoucherValues, RI.Repositories.VoucherValues>();
            services.AddTransient<R.IIssuerTransactions, RI.Repositories.IssuerTransactions>();
            services.AddTransient<R.IHolderTransactions, RI.Repositories.HolderTransactions>();
            services.AddTransient<R.IUserAccounts, RI.Repositories.UserAccounts>();

            services.AddTransient<M.FactorFactory>();
            services.AddTransient<M.IMFCodeGenerator, MI.StubMFCodeGenerator>();
            services.AddTransient<M.IAuthUserFactors, MI.Repositories.AuthUserFactors>();
            services.AddTransient<M.IUpdateEmailFactors, MI.Repositories.UpdateEmailFactors>();
            services.AddTransient<M.IUpdatePasswordFactors, MI.Repositories.UpdatePasswordFactors>();

            services.AddTransient<A.TransactionFactory>();

            services.AddTransient<U.UserCredentialsFactory>();

            services.AddTransient<V.VoucherValueDetailFactory>();
            services.AddTransient<V.VoucherFactory>();


            services.AddTransient<Application.IAdminService, Application.AdminService>();
            services.AddTransient<Application.IUserService, Application.UserService>();
            services.AddTransient<Application.IValuesService, Application.ValuesService>();
            services.AddTransient<Application.IAccountingService, Application.AccountingService>();
            services.AddTransient<Application.IReportsService, Application.ReportsService>();
            services.AddTransient<Application.Commands.DTOFactory>();

            services.AddTransient<Model.IAdminService, AdminService>();
            services.AddTransient<Model.IUserService, UserService>();
            services.AddTransient<Model.IValuesService, ValuesService>();
            services.AddTransient<Model.IAccountingService, AccountingService>();
            services.AddTransient<Model.IReportsService, ReportsService>();

            services.AddDbContext<VouchersContext>(options => options.UseSqlServer("server=localhost;database=Vouchers;trusted_connection=true;").EnableSensitiveDataLogging(), ServiceLifetime.Scoped);
            serviceProvider = services.BuildServiceProvider();

            var dtoFactory = serviceProvider.GetRequiredService<Application.Commands.DTOFactory>();

            AuthViewModel authViewModel = new AuthViewModel(new ServiceFactory(serviceProvider));
            AuthWindow authWindow = new AuthWindow();
            authWindow.DataContext = authViewModel;
            authWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"An unhandled exception occured: {e.Exception.Message}", "Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Warning);
            e.Handled = true;
        }
    }
}
