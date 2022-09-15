using System;
using System.Net.Mail;

namespace Vouchers.MF
{
    public class UpdatePasswordFactor
    {
        public int UserId { get; }
        public string UserPasswordHash { get; }

        public int Code { get; }
        public DateTime CreationTime { get; }
        

        public UpdatePasswordFactor(int userId, string userPasswordHash, int code) {
            UserId = userId;
            UserPasswordHash = userPasswordHash;
            Code = code;

            CreationTime = DateTime.Now;
        }
    }
}
