using System.Net;

namespace Identity.API.Infrastructure.Errors;

public class AppExceptionEnum
{
    public static readonly AppException UserOrPasswordIncorrect = new("Username or password is incorrect");
    public static readonly AppException UserNotFound = new("User not found", HttpStatusCode.NotFound);
}