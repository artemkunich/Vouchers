using AutoMapper;
using Vouchers.Application.Dtos;
using Vouchers.Identities;

namespace Vouchers.MVC.Services
{ 
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<IdentityDetail, IdentityDetailDto>();
        }
    }
}

