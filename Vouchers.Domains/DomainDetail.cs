using System;
using Vouchers.Core;

namespace Vouchers.Domains
{
    public class DomainDetail
    {
        public Guid Id { get; }

        public DomainContract Contract { get; }

        public DateTime CreatedDateTime { get; }

        public string Description { get; set; }

        public bool IsPublic { get; set; }

        public static DomainDetail Create(DomainContract contract, string description) =>
            new DomainDetail(Guid.NewGuid(), contract, description, DateTime.Now);

        internal DomainDetail(Guid id, DomainContract contract, string description, DateTime createdDateTime)
        {
            Id = id;
            Contract = contract;           
            Description = description;
            CreatedDateTime = createdDateTime;
        }

        private DomainDetail() 
        { }
    }
}
