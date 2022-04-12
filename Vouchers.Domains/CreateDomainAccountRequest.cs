using System;
using Vouchers.Core;

namespace Vouchers.Domains
{
    public class CreateDomainAccountRequest
    {
        public Guid Id { get; }

        public Domain Domain { get; }

        public Identity Identity { get; }

        public DateTime CreatedDate { get; }

        public DomainAccount _account;
        public DomainAccount Account {
            get => _account;  
            set 
            {
                if (_account != null)
                    throw new DomainsException("Request is already confirmed");
                _account = value;
            }
        }

        public DomainAccount _processedBy;
        public DomainAccount ProcessedBy
        {
            get => _processedBy;
            set
            {
                if (_processedBy != null)
                    throw new DomainsException("Request is already confirmed");
                _processedBy = value;
            }
        }

        public static CreateDomainAccountRequest Create(Domain domain, Identity identity) =>
            new CreateDomainAccountRequest(Guid.NewGuid(), domain, identity, DateTime.Now);

        internal CreateDomainAccountRequest(Guid id, Domain domain, Identity identity, DateTime createdDate)
        {
            Id = id;
            Domain = domain;
            Identity = identity;
            CreatedDate = createdDate;
        }
    }
}
