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
            services.AddScoped<IRepository<CollectionRequest>, CollectionRequestRepository>();
            services.AddScoped<IRepository<DomainAccount>, DomainAccountRepository>();
            services.AddScoped<IRepository<DomainContract>, DomainContractRepository>();
            services.AddScoped<IRepository<DomainOffersPerIdentityCounter>, DomainOffersPerIdentityCounterRepository>();
            services.AddScoped<IRepository<Domain>, DomainRepository>();
            services.AddScoped<IRepository<Login>, LoginRepository>();
            services.AddScoped<IRepository<Unit>, UnitRepository>();
            services.AddScoped<IRepository<UnitType>, UnitTypeRepository>();

            services.AddScoped<IRepository<Account>, Repository<Account>>();
            services.AddScoped<IRepository<HolderTransaction>, Repository<HolderTransaction>>();
            services.AddScoped<IRepository<HolderTransactionItem>, Repository<HolderTransactionItem>>();
            services.AddScoped<IRepository<IssuerTransaction>, Repository<IssuerTransaction>>();
            services.AddScoped<IRepository<DomainOffer>, Repository<DomainOffer>>();
            services.AddScoped<IRepository<VoucherValue>, Repository<VoucherValue>>();
            services.AddScoped<IRepository<Identity>, Repository<Identity>>();
            services.AddScoped<IRepository<AppImage>, Repository<AppImage>>();
        }
    }
}
