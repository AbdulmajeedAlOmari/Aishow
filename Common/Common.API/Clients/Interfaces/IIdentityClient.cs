using Common.API.Models.Entities;

namespace Common.API.Clients.Interfaces;

public interface IIdentityClient
{
    Task<CommonUserDto> GetUser();
}