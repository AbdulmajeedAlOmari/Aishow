namespace Common.API.Exceptions;

public class ApiException : Exception
{
    public ErrorResponse Error { get; }

    public ApiException(ErrorResponse error) : base(error.ErrorLog)
    {
        Error = error;
    }
}
