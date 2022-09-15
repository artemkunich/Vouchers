using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.WPF.Model;
using Vouchers.WPF.Mono.Extensions;

namespace Vouchers.WPF.Mono
{
    public class UserService : IUserService
    {
        Application.IUserService service;

        public UserService(Application.IUserService service)
        {
            this.service = service;
        }



        public void SendCodeToGetUserAccount(string email)
        {
            service.SendCodeToGetUserAccount(email);
        }

        public UserAccount GetUserAccount(int code, string email, string password)
        {
            var authUser = service.GetUserAccount(code, email, password);
            return authUser.GetUserAccount();
        }



        public void SendCodeToCreateUserAccount(string userAccountId, string email, string password)
        {
            service.SendCodeToCreateUserAccount(userAccountId, email, password);
        }

        public UserAccount CreateUserAccount(int code, string email)
        {
            var authUser = service.CreateUserAccount(code, email);
            return authUser.GetUserAccount();
        }



        public void SendCodeToResetPassword(string currentPassword, string newPassword, UserAccount authUser)
        {
            service.SendCodeToResetPassword(currentPassword, newPassword, authUser.GetUserAccount());
        }

        public UserAccount ResetPassword(int code, UserAccount authUser)
        {
            var updatedAuthUser = service.ResetPassword(code, authUser.GetUserAccount());
            return updatedAuthUser.GetUserAccount();
        }



        public void SendCodeToUpdateEmail(string email, UserAccount authUser)
        {
            service.SendCodeToUpdateEmail(email, authUser.GetUserAccount());
        }

        public UserAccount UpdateEmail(int code, UserAccount authUser)
        {
            var updatedAuthUser = service.UpdateEmail(code, authUser.GetUserAccount());
            return updatedAuthUser.GetUserAccount();
        }



        public Action OnDispose { get; set; }

        public void Dispose()
        {
            OnDispose?.Invoke();
        }

    }
}
