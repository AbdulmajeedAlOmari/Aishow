using System.Net;
using Common.API.Exceptions;

namespace Identity.API.Infrastructure.Errors;

public class AppExceptionEnum
{
    public static readonly ApiException UserOrPasswordIncorrect = new ApiException(
        new ErrorResponse(
            HttpStatusCode.BadRequest,
            "Username or password is incorrect",
            "اسم المستخدم أو كلمة المرور ليست صحيحة"));

    public static readonly ApiException UserNotFound = new ApiException(
        new ErrorResponse(
            HttpStatusCode.BadRequest,
            "User does not exist",
            "المستخدم غير موجود"));
}