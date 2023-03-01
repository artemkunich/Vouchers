using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Queries;

[ApplicationRoles(ApplicationRole.User)]
public sealed class HolderVouchersQuery : ListQuery, IRequest<IReadOnlyList<VoucherDto>>
{
    public Guid ValueId { get; set; }
}