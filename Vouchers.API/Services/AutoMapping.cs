using AutoMapper;
using Vouchers.Application.Dtos;
using Vouchers.Identities;

namespace Vouchers.API.Services;

public sealed class AutoMapping : Profile
{
    public AutoMapping()
    {
        CreateMap<Identity, IdentityDetailDto>();
    }
}