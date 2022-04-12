using System;
using System.Net.Mail;

namespace Vouchers.Auth
{
    public class Session
    {
        public string Domain { get; }
        public string Nickname { get; }
        public Guid TokenId { get; }
        public int UserId { get; }

        public DateTime CreationTime { get; }


        public Session(string domain, string nickname, Guid tokenId, int userId)
        {
            Domain = domain;
            Nickname = nickname;
            TokenId = tokenId;
            UserId = userId;

            CreationTime = DateTime.Now;
        }
    }
}
