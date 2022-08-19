using Common.API.Clients.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Common.API.Clients.Http;

public class HttpClient : IServiceClient
{
    public readonly IIdentityClient Identities;

    public HttpClient(IConfiguration configuration, ILogger<HttpClient> logger, System.Net.Http.HttpClient client)
    {
        Identities = new IdentityClient(configuration, logger, client);
    }
}