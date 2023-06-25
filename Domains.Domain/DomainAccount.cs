using System;
using Akunich.Domain.Abstractions;

namespace Vouchers.Domains.Domain;

public sealed class DomainAccount : AggregateRoot<Guid>
{
    public Guid IdentityId { get; init; }

    public Guid DomainId { get; init; }
    public Domain Domain { get; init; }

    public DateTime CreatedDateTime { get; init; }

    public bool IsIssuer { get; set; }

    public bool IsAdmin { get; set; }

    public bool IsOwner => IdentityId == Domain.Contract.PartyId;

    public bool IsConfirmed { get; set; }

    public static DomainAccount Create(Guid accountId, Guid identityId, Domain domain, DateTime createdDateTime) => new()
    {
        Id = accountId,
        IdentityId = identityId,

        DomainId = domain.Id,
        Domain = domain,

        CreatedDateTime = createdDateTime,
    };

    public bool Equals(DomainAccount domainAccount) =>
        Id == domainAccount.Id;

    public bool NotEquals(DomainAccount domainAccount) =>
        Id != domainAccount.Id;
}

