using System;
using System.Net.Mail;
using Vouchers.Application;

namespace Vouchers.Auth
{
    public class AuthUserFactor
    {
        public UserCredentials UserCredentials { get; }

        public string Code { get; }
        public DateTime CreationTime { get; }
        

        public AuthUserFactor(UserCredentials userCredentials, string code) {
            UserCredentials = userCredentials;
            Code = code;

            CreationTime = DateTime.Now;
        }
    }
}
