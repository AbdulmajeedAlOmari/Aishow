using System.Globalization;
using System.Net;

namespace Identity.API.Infrastructure.Errors;

// custom exception class for throwing application specific exceptions (e.g. for validation) 
// that can be caught and handled within the application
public class AppException : Exception
{
    public HttpStatusCode Status { get; set; } = HttpStatusCode.BadRequest;

    public AppException() : base() {}

    public AppException(string message) : base(message) { }

    public AppException(string message, HttpStatusCode status) : base(message)
    {
        Status = status;
    }

    public AppException(string message, params object[] args) 
        : base(String.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
}
