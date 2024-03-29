﻿using Common.API.Clients.Interfaces;
using Common.API.Helpers;
using Common.API.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Common.API.Authorization;

public class CommonJwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceClient _serviceClient;

    public CommonJwtMiddleware(RequestDelegate next, IServiceClient serviceClient)
    {
        _next = next;
        _serviceClient = serviceClient;
    }

    public async Task Invoke(HttpContext context)
    {
        // Extract bearer token from request headers before contacting Identity service
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        // In case there were no token given, no need to get user details from Identity service
        if (token == null)
        {
            await _next(context);
        }

        // Attempt fetching user details in case exists
        CommonUserDto userDto = await _serviceClient.GetUser();

        // Attach user information to context on successful jwt validation
        context.Items["User"] = userDto;

        await _next(context);
    }
}