using Common.API.Clients.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Common.API.Clients.Http;

public class IdentityClient : HttpClientBase, IIdentityClient
{
    public IdentityClient(
        IConfiguration configuration,
        ILogger logger,
        System.Net.Http.HttpClient client
    ) : base(configuration, logger, client, "BaseUrls:Identity") { }

    public async Task ValidateToken()
    {
        await GetAsync<string>("/api/auth/validate");
    }
}