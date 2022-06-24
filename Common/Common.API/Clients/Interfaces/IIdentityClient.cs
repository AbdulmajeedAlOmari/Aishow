namespace Common.API.Clients.Interfaces;

public interface IIdentityClient
{
    Task ValidateToken();
}