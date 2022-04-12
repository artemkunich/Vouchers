using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using Vouchers.Application;
using Vouchers.Application.Infrastructure;

namespace Vouchers.Auth
{
    public class TokenService : ITokenService
    {
        private readonly IUsers users;
        private readonly ISessions sessions;
        
        internal TokenService(IUsers users, ISessions sessions)
        {
            this.users = users;
            this.sessions = sessions;
        }

        public User GetUser(Token token) 
        {

            var session = sessions.GetSession(token.Id);
            if (token.Id != session.TokenId)
                throw new ApplicationException("Token Id is not valid");

            return users.GetById(session.UserId);
        }

    }
}
