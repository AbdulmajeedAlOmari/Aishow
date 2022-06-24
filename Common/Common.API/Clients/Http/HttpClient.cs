using Common.API.Clients.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Common.API.Clients.Http;

public class HttpClient : IServiceClient
{
    private readonly IIdentityClient _identityClient;

    public HttpClient(IConfiguration configuration, ILogger<HttpClient> logger, System.Net.Http.HttpClient client)
    {
        _identityClient = new IdentityClient(configuration, logger, client);
    }

    public Task ValidateToken() => _identityClient.ValidateToken();
}