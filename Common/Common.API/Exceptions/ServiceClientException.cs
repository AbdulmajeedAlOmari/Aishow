using EasyNetQ.SystemMessages;
using Newtonsoft.Json;

namespace Common.API.Exceptions;

public class ServiceClientException : Exception
{
    public HttpResponseMessage Response { get; }
    public string TextContent { get; }
    public Error Error { get; }

    public ServiceClientException(HttpResponseMessage response, string textContent, Error error = null)
        : base($"ServiceClientException: status {response.StatusCode} and content {textContent}")
    {
        Response = response;
        TextContent = textContent;
        Error = error;
    }

    public T DeserializeResponseAs<T>()
    {
        return JsonConvert.DeserializeObject<T>(TextContent);
    }
}
