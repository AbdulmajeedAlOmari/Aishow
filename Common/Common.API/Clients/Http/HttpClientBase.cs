using System.Text;
using Common.API.Exceptions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Common.API.Clients.Http;

public class HttpClientBase
{
    private readonly System.Net.Http.HttpClient _client;
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;
    private readonly string _baseUrl;


    public HttpClientBase(IConfiguration configuration, ILogger logger, System.Net.Http.HttpClient client, string baseUrlConfigKey)
    {
        _client = client;
        _configuration = configuration;
        _logger = logger;
        _baseUrl = configuration[baseUrlConfigKey];
    }

    public async Task<TRes> PostAsync<TReq, TRes>(string path, TReq body)
    {
        string fullPath = _baseUrl;
        fullPath += path;

        return await SendPostAsync<TReq, TRes>(fullPath, body);
    }

    public async Task<TRes> PostAsync<TReq, TRes>(string path, TReq body, Dictionary<string, string> headers)
    {
        string fullPath = _baseUrl;
        fullPath += path;

        return await SendPostAsync<TReq, TRes>(fullPath, body, headers);

    }

    public async Task<TRes> GetAsync<TRes>(string path)
    {
        string fullPath = _baseUrl;
        fullPath += path;

        return await SendGetAsync<TRes>(fullPath);
    }

    public async Task<TRes> GetAsync<TRes>(string path, Dictionary<string, string> parameters)
    {
        string fullPath = _baseUrl;
        fullPath += path;

        string url = QueryHelpers.AddQueryString(fullPath, parameters);
        return await SendGetAsync<TRes>(url);
    }

    private async Task<TRes> SendPostAsync<TReq, TRes>(string fullPath, TReq body,
        Dictionary<string, string> headers = null)
    {
        _logger.LogTrace("Sending HTTP POST request to: {FullPath}", fullPath);

        StringContent payload = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

        HttpRequestMessage message = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(fullPath),
            Content = payload
        };

        if (headers != null)
        {
            foreach ((string key, string value) in headers)
            {
                message.Headers.Add(key, value);
            }
        }

        HttpResponseMessage response = await _client.SendAsync(message);
        string content = await response.Content.ReadAsStringAsync();
        _logger.LogTrace("Received response from {FullPath} with status {ResponseStatusCode} and content {Content}", fullPath, response.StatusCode, content);

        if (!response.IsSuccessStatusCode)
            throw new ServiceClientException(response, content);

        TRes resObject = DeserializeResponse<TRes>(content);
        return resObject;
    }

    private async Task<TRes> SendGetAsync<TRes>(string fullPath)
    {
        _logger.LogTrace("Sending HTTP GET request to {FullPath}", fullPath);

        HttpResponseMessage response = await _client.GetAsync(fullPath);
        string content = await response.Content.ReadAsStringAsync();
        _logger.LogTrace("Received response from {FullPath} with status {ResponseStatusCode} and content {Content}", fullPath, response.StatusCode, content);

        if (!response.IsSuccessStatusCode)
            throw new ServiceClientException(response, content);

        TRes resObject = JsonConvert.DeserializeObject<TRes>(content);
        return resObject;
    }

    // Check if the type of response is primitive/simple or complex
    // And returns/deserializes the response accordingly.
    private TRes DeserializeResponse<TRes>(string content)
    {
        Type t = typeof(TRes);
        if (t.IsPrimitive || t == typeof(decimal) || t == typeof(string))
            return (TRes)(object)content;

        return JsonConvert.DeserializeObject<TRes>(content);
    }
}