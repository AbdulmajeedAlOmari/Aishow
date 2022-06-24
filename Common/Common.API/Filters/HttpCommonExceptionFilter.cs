using System.Net;
using Common.API.Clients.Extensions;
using Common.API.Enums;
using Common.API.Exceptions;
using EasyNetQ.SystemMessages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Sentry;

namespace Common.API.Filters;

public class HttpCommonExceptionFilter : ExceptionFilterAttribute
{
    private readonly ILogger _logger;

    [ActivatorUtilitiesConstructor]
    public HttpCommonExceptionFilter(ILogger<HttpCommonExceptionFilter> logger)
    {
        _logger = logger;
    }

    public override void OnException(ExceptionContext context)
    {
        string entityId = context.HttpContext.User?.FindFirst("UserId")?.Value ?? "0";

        // Fill user id information to Sentry tag
        SentrySdk.ConfigureScope(scope =>
        {
            scope.SetTag("UserId", entityId);
        });

        if (context.Exception.GetType() == typeof(ApiException))
            HandleApiException(context);
        else
            HandleOtherExceptions(context);
    }

    private void HandleApiException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, context.Exception.Message);

        context.HttpContext.Request.Headers.TryGetValue("Accept-Language", out StringValues languageHeader);
        string lang = languageHeader;

        // Set error message to arabic if lang is arabic
        ApiException ex = (ApiException)context.Exception;

        ProblemDetails res = ex.Error.GetProblemDetailsByLang(lang.ToEnum<LanguageEnum>());
        context.Result = new JsonResult(res);
        context.HttpContext.Response.StatusCode = res.Status ?? (int)HttpStatusCode.InternalServerError;

        context.ExceptionHandled = true;
    }

    private void HandleOtherExceptions(ExceptionContext context)
    {
        _logger.LogCritical(context.Exception, context.Exception.Message);

        context.HttpContext.Request.Headers.TryGetValue("Accept-Language", out StringValues languageHeader);
        string lang = languageHeader;

        string messageAr = "حدث خطأ أثناء معالجة طلبك. الرجاء المحاولة لاحقاً";
        string messageEn = "An error has occured while processing your request. Please try again later.";
        int statusCode = (int)HttpStatusCode.InternalServerError;
        var json = new ProblemDetails
        {
            Status = statusCode,
            Type = "about:blank",
            Title = lang == "ar" ? messageAr : messageEn,
            Detail = lang == "ar" ? messageAr : messageEn,
        };

        context.Result = new JsonResult(json);
        context.HttpContext.Response.StatusCode = statusCode;

        context.ExceptionHandled = true;

        // Capture unhandled exception to Sentry
        SentrySdk.CaptureException(context.Exception);
    }
}