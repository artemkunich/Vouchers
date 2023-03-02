using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Queries;

public sealed class SubscribersQuery : ListQuery, IRequest<IReadOnlyList<SubscriberDto>>
{
    [Required]
    public Guid DomainId { get; set; }

    public string IdentityName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}