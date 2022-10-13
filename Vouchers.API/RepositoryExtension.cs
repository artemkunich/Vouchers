using Microsoft.Extensions.DependencyInjection;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.UseCases;
using Vouchers.Core;
using Vouchers.Domains;
using Vouchers.EntityFramework.Repositories;
using Vouchers.Files;
using Vouchers.Identities;
using Vouchers.Values;

namespace Vouchers.API
{
    public static class RepositoryExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<AccountItem>,AccountItemRepository>();
            services.AddScoped<IRepository<HolderTransactionRequest>, HolderTransactionRequestRepository>();
            services.AddScoped<IRepository<DomainAccount>, DomainAccountRepository>();
            services.AddScoped<IRepository<DomainContract>, DomainContractRepository>();
            services.AddScoped<IRepository<DomainOffersPerIdentityCounter>, DomainOffersPerIdentityCounterRepository>();
            services.AddScoped<IRepository<Domain>, DomainRepository>();
            services.AddScoped<IRepository<Login>, LoginRepository>();
            services.AddScoped<IRepository<Unit>, UnitRepository>();
            services.AddScoped<IRepository<UnitType>, UnitTypeRepository>();

            services.AddScoped<IRepository<Account>, GenericRepository<Account>>();
            services.AddScoped<IRepository<HolderTransaction>, GenericRepository<HolderTransaction>>();
            services.AddScoped<IRepository<HolderTransactionItem>, GenericRepository<HolderTransactionItem>>();
            services.AddScoped<IRepository<IssuerTransaction>, GenericRepository<IssuerTransaction>>();
            services.AddScoped<IRepository<DomainOffer>, GenericRepository<DomainOffer>>();
            services.AddScoped<IRepository<VoucherValue>, GenericRepository<VoucherValue>>();
            services.AddScoped<IRepository<Identity>, GenericRepository<Identity>>();
            services.AddScoped<IRepository<CroppedImage>, GenericRepository<CroppedImage>>();
        }
    }
}
