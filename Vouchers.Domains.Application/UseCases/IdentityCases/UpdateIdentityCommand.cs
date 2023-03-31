using Microsoft.AspNetCore.Http;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Domains.Application.Dtos;
using Vouchers.Domains.Application.Permissions;

namespace Vouchers.Domains.Application.UseCases.IdentityCases;

[IdentityPermission]
public sealed class UpdateIdentityCommand : IRequest<Unit>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public IFormFile Image { get; set; }

    public CropParametersDto CropParameters { get; set; }

}