using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Entities;

namespace Vouchers.Identities
{
    public sealed class Login : Entity<Guid>
    {
        public string LoginName { get; }

        public Guid IdentityId { get; }
        public Identity Identity { get; }

        public static Login Create(string loginName, Identity identity) =>
            new Login(Guid.NewGuid(), loginName, identity);

        internal Login(Guid id, string loginName, Identity identity) : base(id)
        {
            LoginName = loginName;
            IdentityId = identity.Id;
            Identity = identity;
        }

        private Login() { }
    }
}
