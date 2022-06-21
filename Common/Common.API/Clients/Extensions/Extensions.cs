using Common.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Sentry;
using Sentry.AspNetCore;

namespace Common.Clients.Extensions;

public static class Extensions
{
    /// <summary>
    /// Uses Aishow Customized Sentry integration.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="configureOptions">The configure options.</param>
    /// <returns></returns>
    public static IWebHostBuilder UseAishowSentry(
        this IWebHostBuilder builder,
        Action<SentryAspNetCoreOptions> configureOptions)
    {
        return builder.UseSentry(configureOptions: (context, options) =>
        {
            options.AddExceptionFilterForType<ApiException>();
            configureOptions?.Invoke(options);
        });
    }
}