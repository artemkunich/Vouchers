using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.WPF.Model
{
    public interface IServiceFactory
    {
        IAdminService CreateAdminService();


        IUserService CreateUserService();
        IValuesService CreateValuesService();
        IAccountingService CreateAccountingService();
        IReportsService CreateReportsService();
    }
}
