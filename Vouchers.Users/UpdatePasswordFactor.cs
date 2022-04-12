using System;
using System.Net.Mail;

namespace Vouchers.Auth
{
    public class UpdatePasswordFactor
    {
        public int UserId { get; }
        public string UserPasswordHash { get; }

        public string Code { get; }
        public DateTime CreationTime { get; }
        

        public UpdatePasswordFactor(int userId, string userPasswordHash, string code) {
            UserId = userId;
            UserPasswordHash = userPasswordHash;
            Code = code;

            CreationTime = DateTime.Now;
        }
    }
}
