using AutoMapper;
using Vouchers.Domains.Application.Dtos;
using Vouchers.Domains.Domain;


namespace Vouchers.API.Services;

public sealed class AutoMapping : Profile
{
    public AutoMapping()
    {
        CreateMap<Identity, IdentityDetailDto>();
    }
}