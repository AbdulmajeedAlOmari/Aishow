using System.Runtime.Serialization;
using Common.API.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Sentry;
using Sentry.AspNetCore;

namespace Common.API.Clients.Extensions;

public static class Extensions
{
    public static T ToEnum<T>(this string str)
    {
        Type enumType = typeof(T);
        foreach (string name in Enum.GetNames(enumType))
        {
            EnumMemberAttribute enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
            if (enumMemberAttribute.Value == str) return (T)Enum.Parse(enumType, name);
        }

        return default;
    }

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

    public static IWebHostBuilder UseAishowSentry(
        this IWebHostBuilder builder)
    {
        return builder.UseSentry(configureOptions: (context, options) =>
        {
            options.AddExceptionFilterForType<ApiException>();
            options.SampleRate = 1;
        });
    }
}