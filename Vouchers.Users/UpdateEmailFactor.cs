using System;
using System.Net.Mail;

namespace Vouchers.Auth
{
    public class UpdateEmailFactor
    {
        public int UserId { get; }
        public string UserEmail { get; }

        public string Code { get; }
        public DateTime CreationTime { get; }
        

        public UpdateEmailFactor(int userId, string userEmail, string code) {
            UserId = userId;
            UserEmail = userEmail;
            Code = code;

            CreationTime = DateTime.Now;
        }
    }
}
