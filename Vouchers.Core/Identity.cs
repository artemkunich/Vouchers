using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Core
{
    public class Identity
    {
        public Guid Id { get; }

        public static Identity Create() =>
            new Identity(Guid.NewGuid());

        internal Identity(Guid id) : this() =>
            Id = id;

        private Identity()
        { }

        public bool Equals(Identity identity)
        {
            return Id == identity.Id;
        }

        public bool NotEquals(Identity identity)
        {
            return Id != identity.Id;
        }
    }
}
