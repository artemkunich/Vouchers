using System;
using System.Collections.Generic;
using System.Linq;
using Vouchers.Primitives;

namespace Vouchers.Identities;

public sealed class Identity : AggregateRoot<Guid>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public Guid? ImageId { get; set; }

    private Identity() { }

    public static Identity Create(string email, string firstName, string lastName) =>
        new Identity(Guid.NewGuid(), email, firstName, lastName);

    internal Identity(Guid id, string email, string firstName, string lastName) : base(id)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }

    public bool Equals(Identity identity) =>
        Id == identity.Id;

    public bool NotEquals(Identity identity) =>
        Id != identity.Id;

}