using System.Net;
using System.Security.Claims;
using Common.API.Exceptions;
using EasyNetQ.SystemMessages;
using Microsoft.AspNetCore.Mvc;

namespace Common.API.Helpers;

public class CommonControllerBase : ControllerBase
{
    public CommonControllerBase()
    {
    }

    public string Language => Request.Headers["Accept-Language"];

    public string UserId
    {
        get
        {
            var userIdClaim = User.FindFirst("id");

            if (userIdClaim == null)
            {
                throw NotAuthenticatedException();
            }

            return userIdClaim.Value;
        }
    }

    public string Email
    {
        get
        {
            var emailClaim = User.FindFirst("email");

            if (emailClaim == null)
            {
                throw NotAuthenticatedException();
            }

            return emailClaim.Value;
        }
    }

    public string Username
    {
        get
        {
            var usernameClaim = User.FindFirst("unique_name");

            if (usernameClaim == null)
            {
                throw NotAuthenticatedException();
            }

            return usernameClaim.Value;
        }
    }

    private ApiException NotAuthenticatedException()
    {
        return new ApiException(new ErrorResponse(
            HttpStatusCode.Unauthorized,
            "User not authenticated", 
            "الرجاء تسجيل الدخول"
        ));
    }
}