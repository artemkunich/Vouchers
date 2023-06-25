﻿using System;
using Akunich.Domain.Abstractions;

namespace Vouchers.Domains.Domain;

public sealed class Identity : AggregateRoot<Guid>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    private Identity() { }

    public static Identity Create(Guid id, string email, string firstName, string lastName) => new()
    {
        Id = id,
        Email = email,
        FirstName = firstName,
        LastName = lastName
    };

}