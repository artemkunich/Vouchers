using System;
using System.Net.Mail;
using Vouchers.Auth;

namespace Vouchers.MF
{
    public class AuthUserFactor
    {
        public UserCredentials UserAccount { get; }

        public int Code { get; }
        public DateTime CreationTime { get; }
        

        public AuthUserFactor(UserCredentials userAccount, int code) {
            UserAccount = userAccount;
            Code = code;

            CreationTime = DateTime.Now;
        }
    }
}
