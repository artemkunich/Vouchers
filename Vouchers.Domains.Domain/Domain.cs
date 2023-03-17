using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Domains.Domain.Properties;
using Vouchers.Primitives;

namespace Vouchers.Domains.Domain;

public sealed class Domain : AggregateRoot<Guid>
{
    public Guid ContractId { get; init; }
    public DomainContract Contract { get; init; }

    public decimal Credit  { get; private set; }

    public int MembersCount { get; private set; }

    public DateTime CreatedDateTime { get; init; }

    public string Description { get; set; }

    public bool IsPublic { get; set; }

    public Guid? ImageId { get; set; }

    public static Domain Create(Guid id, DomainContract contract, decimal credit, int membersCount, DateTime createdDateTime) => new()
    {
        Id = id,
        ContractId = contract.Id,
        Contract = contract,

        Credit = credit,
        MembersCount = membersCount,
        CreatedDateTime = createdDateTime
    };

    public void IncreaseMembersCount()
    {
        if (MembersCount + 1 > Contract.Offer.MaxMembersCount)
            throw new DomainsException("Domain max members count exceeded");

        MembersCount++;
    }
    public void DecreaseMembersCount()
    {
        if (MembersCount <= 0)
            throw new DomainsException(Resources.DomainHasNotAnyMember);

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