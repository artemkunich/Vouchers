using AutoMapper;
using Vouchers.Application.Dtos;
using Vouchers.Identities.Domain;

namespace Vouchers.API.Services;

public sealed class AutoMapping : Profile
{
    public AutoMapping()
    {
        CreateMap<Identity, IdentityDetailDto>();
    }
}