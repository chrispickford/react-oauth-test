using AutoMapper;
using ReactOAuthTest.Api.Dto;
using ReactOAuthTest.Data.Entities;

namespace ReactOAuthTest.Api.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AccountRegisterDto, User>()
                .ForMember(dst => dst.UserName, opts => opts.MapFrom(src => src.Email));
            CreateMap<AccountLoginDto, User>()
                .ForMember(dst => dst.UserName, opts => opts.MapFrom(src => src.Email));
        }
    }
}