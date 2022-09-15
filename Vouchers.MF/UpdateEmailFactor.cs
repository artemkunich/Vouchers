using System;
using System.Net.Mail;

namespace Vouchers.MF
{
    public class UpdateEmailFactor
    {
        public int UserId { get; }
        public string UserEmail { get; }

        public int Code { get; }
        public DateTime CreationTime { get; }
        

        public UpdateEmailFactor(int userId, string userEmail, int code) {
            UserId = userId;
            UserEmail = userEmail;
            Code = code;

            CreationTime = DateTime.Now;
        }
    }
}
