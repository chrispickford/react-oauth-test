using AutoMapper;
using ReactOAuthTest.Api.Dto;
using ReactOAuthTest.Data.Entities;

namespace ReactOAuthTest.Api.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}