using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.WPF.Model;

namespace Vouchers.WPF.Mono
{
    public class ServiceFactory : IServiceFactory
    {
        IServiceProvider serviceProvider;

        public ServiceFactory(IServiceProvider serviceProvider) {
            this.serviceProvider = serviceProvider;
        }

        public IAccountingService CreateAccountingService()
        {
            var scope = serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IAccountingService>();
            service.OnDispose += () => scope.Dispose();
            return service;
        }

        public IUserService CreateUserService()
        {
            var scope = serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IUserService>();
            service.OnDispose += () => scope.Dispose();
            return service;
        }

        public IAdminService CreateAdminService()
        {
            var scope = serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IAdminService>();
            service.OnDispose += () => scope.Dispose();
            return service;
        }

        public IValuesService CreateValuesService()
        {
            var scope = serviceProvider.CreateScope();            
            var service = scope.ServiceProvider.GetRequiredService<IValuesService>();
            service.OnDispose += () => scope.Dispose();
            return service;
        }

        public IReportsService CreateReportsService()
        {
            var scope = serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IReportsService>();
            service.OnDispose += () => scope.Dispose();
            return service;
        }
    }
}
