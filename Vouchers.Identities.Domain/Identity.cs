﻿using System;
using System.Collections.Generic;
using System.Linq;
using Vouchers.Primitives;

namespace Vouchers.Identities.Domain;

public sealed class Identity : AggregateRoot<Guid>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public Guid? ImageId { get; set; }

    private Identity() { }

    public static Identity Create(Guid id, string email, string firstName, string lastName) => new()
    {
        Id = id,
        Email = email,
        FirstName = firstName,
        LastName = lastName
    };

}