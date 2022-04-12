using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace Vouchers.Core
{
    public class DomainAccount
    {
        public Guid Id { get; }

        public Identity Identity { get; }

        public Domain Domain { get; }

        public DateTime CreatedDateTime { get; }

        public bool IsIssuer { get; set; }

        public bool IsAdmin { get; set; }

        public decimal Supply { get; private set; }


        public static DomainAccount Create(Identity identity, Domain domain, bool isIssuer, bool isAdmin) =>
            new DomainAccount(Guid.NewGuid(), identity, domain, DateTime.Now, isIssuer, isAdmin);

        internal DomainAccount(Guid id, Identity identity, Domain domain, DateTime createdDateTime, bool isIssuer, bool isAdmin) : this(identity, domain, createdDateTime, isIssuer, isAdmin) =>
            Id = id;

        internal DomainAccount(Identity identity, Domain domain, DateTime createdDateTime, bool isIssuer, bool isAdmin)
        {
            Identity = identity;
            Domain = domain;
            CreatedDateTime = createdDateTime;

            IsIssuer = isIssuer;
            IsAdmin = isAdmin;
        }

        private DomainAccount() { }


        public bool Equals(DomainAccount domainAccount) {
            return Id == domainAccount.Id;
        }

        public bool NotEquals(DomainAccount domainAccount) {
            return Id != domainAccount.Id;
        }

        public void IncreaseSupply(decimal amount) { 
            if(amount <= 0)
                throw new CoreException($"Supply cannot be changed by 0 or negative amount");
            Supply += amount;
        }

        public void ReduceSupply(decimal amount)
        {
            if (amount <= 0)
                throw new CoreException($"Supply cannot be changed by 0 or negative amount");
            if (Supply < amount)
                throw new CoreException($"Detected attempt to set negative user's supply");
            Supply -= amount;
        }

        public bool CanBeRemoved()
        {
            return Supply == 0;
        }
    }
}
