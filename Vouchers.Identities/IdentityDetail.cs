using System;
using System.Collections.Generic;
using System.Linq;
using Vouchers.Core;

namespace Vouchers.Identities
{
    public class IdentityDetail {

        public Guid Id { get; }

        public Identity Identity { get; }

        public string IdentityName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        private IdentityDetail(){ }

        public static IdentityDetail Create(Identity identity, string identityName, string email) =>
            new IdentityDetail(Guid.NewGuid(), identity, identityName, email);

        internal IdentityDetail(Guid id, Identity identity, string identityName, string email) : this(identity, identityName, email) =>
            Id = id;

        internal IdentityDetail(Identity identity, string identityName, string email)
        {
            Identity = identity;
            IdentityName = identityName;
            Email = email;
        }

        
    }
}
