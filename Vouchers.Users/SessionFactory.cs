using System;
using System.Net.Mail;

namespace Vouchers.Auth
{
    public class SessionFactory
    {        
        public Session CreateSession(string domain, string nickname, Guid tokenId, int userId)
        {
            return new Session(domain, nickname, tokenId, userId);
        }

        public Session CreateSession(string domain, string nickname, int userId)
        {
            return new Session(domain, nickname, Guid.NewGuid(), userId);
        }
    }
}
