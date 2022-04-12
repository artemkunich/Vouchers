using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Core
{
    public class Domain
    {
        public Guid Id { get; }

        public decimal Credit  { get; private set; }

        public int MembersCount { get; private set; }

        public static Domain Create(decimal credit) =>
            new Domain(Guid.NewGuid(), credit, 0);

        internal Domain(Guid id, decimal credit, int membersCount) : this(credit, membersCount) =>
            Id = id;

        internal Domain(decimal credit, int membersCount)
        {
            Credit = credit;
            MembersCount = membersCount;
        }

        private Domain() { }

        internal void IncreaseMembersCount() =>
            MembersCount++;

        internal void DecreaseMembersCount()
        {
            if (MembersCount <= 0)
                throw new CoreException("Domain has not any member");

            MembersCount--;
        }

        internal void IncreaseCredit(decimal amount) =>        
            Credit += amount;

        internal void DecreaseCredit(decimal amount) =>
            Credit -= amount;


        public bool Equals(Domain domain) =>
            Id == domain.Id;

        public bool NotEquals(Domain domain) =>
            Id != domain.Id;
    }
}
