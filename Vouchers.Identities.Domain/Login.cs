using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Primitives;

namespace Vouchers.Identities.Domain;

public sealed class Login : AggregateRoot<Guid>
{
    public string LoginName { get; init; }

    public Guid IdentityId { get; init; }
    public Identity Identity { get; init; }

    public static Login Create(Guid id, string loginName, Identity identity) => new()
    {
        Id = id,
        LoginName = loginName,
        IdentityId = identity.Id,
        Identity = identity,
    };
}

