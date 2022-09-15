using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vouchers.WPF.Model;
using Vouchers.WPF.Mono.Extensions;

namespace Vouchers.WPF.Mono
{
    public class AdminService : IAdminService
    {
        Application.IAdminService service;
        Application.Commands.DTOFactory dtoFactory;

        public AdminService(Application.IAdminService service, Application.Commands.DTOFactory dtoFactory) {
            this.service = service;
            this.dtoFactory = dtoFactory;
        }

        public void UpdatePermissions(string userAccountId, UserPermissions permissions, UserAccount authUser)
        {
            service.UpdatePermissions(
                userAccountId,
                dtoFactory.CreatePermissionsDTO(permissions.IssuerOperations, permissions.HolderOperations, permissions.UserAdministration),
                authUser.GetUserAccount()
            );
        }

        public void ResetPassword(string userAccountId, string newPassword, UserAccount authUser)
        {
            service.ResetPassword(userAccountId, newPassword, authUser.GetUserAccount());
        }

        public void RemoveUserAccount(string userAccountId, UserAccount authUser)
        {
            service.RemoveUserAccount(userAccountId, authUser.GetUserAccount());
        }

        public Action OnDispose { get; set; }

        public void Dispose()
        {
            OnDispose?.Invoke();
        }

        
    }
}
