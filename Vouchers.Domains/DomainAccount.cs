using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using Vouchers.Entities;

namespace Vouchers.Domains
{
    public sealed class DomainAccount : Entity
    {
        public Guid IdentityId { get; }

        public Guid DomainId { get; }
        public Domain Domain { get; }

        public DateTime CreatedDateTime { get; }

        public bool IsIssuer { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsOwner => IdentityId == Domain.Contract.PartyId;

        public bool IsConfirmed { get; set; }

        public static DomainAccount Create(Guid accountId, Guid identityId, Domain domain) =>
            new DomainAccount(accountId, identityId, domain, DateTime.Now);

        internal DomainAccount(Guid accountId, Guid identityId, Domain domain, DateTime createdDateTime) : base(accountId)
        {
            IdentityId = identityId;

            DomainId = domain.Id;
            Domain = domain;
            
            CreatedDateTime = createdDateTime;
        }

        private DomainAccount() { }


        public bool Equals(DomainAccount domainAccount) =>
            Id == domainAccount.Id;

        public bool NotEquals(DomainAccount domainAccount) =>
            Id != domainAccount.Id;
    }
}
