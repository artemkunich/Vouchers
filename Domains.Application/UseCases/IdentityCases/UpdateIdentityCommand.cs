using Akunich.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Vouchers.Domains.Application.Dtos;

namespace Vouchers.Domains.Application.UseCases.IdentityCases;

//[IdentityPermission]
public sealed class UpdateIdentityCommand : IRequest<Unit>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public IFormFile Image { get; set; }

    public CropParametersDto CropParameters { get; set; }

}