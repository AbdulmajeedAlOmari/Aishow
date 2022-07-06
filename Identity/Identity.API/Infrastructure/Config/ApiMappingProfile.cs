using AutoMapper;
using Common.API.Models.Entities;
using Identity.API.Infrastructure.Entities;
using Identity.API.Infrastructure.Models;

namespace Identity.API.Infrastructure.Config;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<RegisterRequest, User>();
        CreateMap<User, CommonUserDto>();
        CreateMap<Role, CommonRoleDto>();
    }
}