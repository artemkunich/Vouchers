using System;
using Akunich.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Vouchers.Domains.Application.Dtos;

namespace Vouchers.Domains.Application.UseCases.DomainCases;

public sealed class UpdateDomainDetailCommand : IRequest<Unit>
{
    public Guid DomainId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public bool? IsPublic { get; set; }

    public CropParametersDto CropParameters { get; set; }

    public IFormFile Image { get; set; }
}