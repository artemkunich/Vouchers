using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using Vouchers.Entities;

namespace Vouchers.Core
{
    public class Account : Entity
    {
        public DateTime CreatedDateTime { get; }
        public decimal Supply { get; private set; }

        public static Account Create() =>
            new Account(Guid.NewGuid(), DateTime.Now);

        internal Account(Guid id, DateTime createdDateTime) : base(id)
        {
            CreatedDateTime = createdDateTime;
        }   

        private Account() { }

        public void IncreaseSupply(decimal amount) 
        { 
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
