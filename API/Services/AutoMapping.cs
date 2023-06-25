using AutoMapper;
using Vouchers.Domains.Application.Dtos;
using Vouchers.Common.Application.Dtos;

namespace Vouchers.API.Services;

public sealed class AutoMapping : Profile
{
    public AutoMapping()
    {
        CreateMap<Identity, IdentityDetailDto>();
    }
}