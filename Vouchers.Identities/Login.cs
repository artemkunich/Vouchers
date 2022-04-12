using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Core;

namespace Vouchers.Identities
{
    public class Login
    {
        public Guid Id { get; }
        public string LoginName { get; }
        public Identity Identity { get; }

        public static Login Create(string loginName, Identity identity)
        {
            return new Login(Guid.NewGuid(), loginName, identity);
        }

        internal Login(Guid id, string loginName, Identity identity)
        {
            Id = id;
            LoginName = loginName;
            Identity = identity;
        }

        private Login() { }
    }
}
