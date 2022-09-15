using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Entities;

namespace Vouchers.Domains
{
    public class Domain : Entity
    {
        public Guid ContractId { get; }
        public DomainContract Contract { get; }

        public decimal Credit  { get; private set; }

        public int MembersCount { get; private set; }

        public DateTime CreatedDateTime { get; }

        public string Description { get; set; }

        public bool IsPublic { get; set; }

        public static Domain Create(DomainContract contract, decimal credit) =>
            new Domain(Guid.NewGuid(), contract, credit, 0, DateTime.Now);

        internal Domain(Guid id, DomainContract contract, decimal credit, int membersCount, DateTime createdDateTime) : base(id)
        {
            ContractId = contract.Id;
            Contract = contract;
            
            Credit = credit;
            MembersCount = membersCount;
            CreatedDateTime = createdDateTime;
        }

        private Domain() { }

        public void IncreaseMembersCount() =>
            MembersCount++;

        public void DecreaseMembersCount()
        {
            if (MembersCount <= 0)
                throw new DomainsException("Domain has not any member");

            MembersCount--;
        }

        public void IncreaseCredit(decimal amount) =>        
            Credit += amount;

        public void DecreaseCredit(decimal amount) =>
            Credit -= amount;


        public bool Equals(Domain domain) =>
            Id == domain.Id;

        public bool NotEquals(Domain domain) =>
            Id != domain.Id;
    }
}
