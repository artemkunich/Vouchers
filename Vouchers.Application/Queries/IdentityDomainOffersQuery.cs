using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Queries;

public sealed class IdentityDomainOffersQuery : ListQuery, IRequest<IReadOnlyList<DomainOfferDto>>
{
}