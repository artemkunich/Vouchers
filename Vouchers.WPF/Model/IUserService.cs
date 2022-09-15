using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.WPF.Model
{
    public interface IUserService : IDisposable
    {
        void SendCodeToGetUserAccount(string email);
        UserAccount GetUserAccount(int code, string email, string password);

        void SendCodeToCreateUserAccount(string userAccountId, string email, string password);
        UserAccount CreateUserAccount(int code, string email);

        void SendCodeToUpdateEmail(string email, UserAccount authUser);
        UserAccount UpdateEmail(int code, UserAccount authUser);

        void SendCodeToResetPassword(string currentPassword, string newPassword, UserAccount authUser);
        UserAccount ResetPassword(int code, UserAccount authUser);

        Action OnDispose { get; set; }
    }
}
