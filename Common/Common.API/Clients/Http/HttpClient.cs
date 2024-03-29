﻿using Common.API.Clients.Interfaces;
using Common.API.Models.Entities;
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

    public Task<CommonUserDto> GetUser() => _identityClient.GetUser();
}