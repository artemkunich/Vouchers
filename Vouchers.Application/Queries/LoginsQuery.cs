using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Queries;

[IdentityRoles(IdentityRole.Admin)]
public sealed class LoginsQuery : ListQuery, IRequest<IReadOnlyList<LoginDto>>
{
    public string LoginName { get; set; }

    public string IdentityName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}