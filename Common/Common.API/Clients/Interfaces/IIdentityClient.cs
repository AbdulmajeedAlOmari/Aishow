using System.Net;

namespace Common.Clients.Interfaces;

public interface IIdentityClient
{
    Task ValidateToken();
}