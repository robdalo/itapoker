using System.Net.Http.Headers;
using System.Text;

namespace itapoker.Shared.Consumers;

public class RestConsumer
{
    private string _baseUrl;

    private const string ContentType = "application/json";

    private static HttpClient _client;

    public RestConsumer(string baseUrl)
    {
        _baseUrl = baseUrl;
        _client = new HttpClient();
    }

    private void ConfigureHeaders(string authToken = "")
    {
        if (!_client.DefaultRequestHeaders.Accept.Any(a => a.MediaType == ContentType))
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType));

        if (!string.IsNullOrEmpty(authToken))
        {
            if (_client.DefaultRequestHeaders.Authorization.Scheme != "bearer")
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authToken);
        }
    }

    public async Task<T> GetAsync<T>(string endpoint, string authToken)
    {
        ConfigureHeaders(authToken);

        var response = await _client.GetAsync($"{_baseUrl}/{endpoint}");
        var content = await response.Content.ReadAsStringAsync();

        return Serializer.Deserialize<T>(content);
    }

    private StringContent GetStringContent(object payload)
    {
        if (payload == null)
            return null;

        return new StringContent(Serializer.Serialize(payload), Encoding.UTF8, ContentType);
    }

    public async Task<T> PostAsync<T>(string endpoint, string authToken, object payload)
    {
        ConfigureHeaders(authToken);

        var content = GetStringContent(payload);
        var response = await _client.PostAsync($"{_baseUrl}/{endpoint}", content);
        var serialized = await response.Content.ReadAsStringAsync();

        return Serializer.Deserialize<T>(serialized);
    }

    public async Task PutAsync(string endpoint, string authToken)
    {
        ConfigureHeaders(authToken);

        await _client.PutAsync($"{_baseUrl}/{endpoint}", null);
    }

    public async Task PutAsync(string endpoint, string authToken, object payload)
    {
        ConfigureHeaders(authToken);

        var content = GetStringContent(payload);
        await _client.PutAsync($"{_baseUrl}/{endpoint}", content);
    }

    public async Task<T> PutAsync<T>(string endpoint, string authToken, object payload)
    {
        ConfigureHeaders(authToken);

        var content = GetStringContent(payload);
        var response = await _client.PutAsync($"{_baseUrl}/{endpoint}", content);
        var serialized = await response.Content.ReadAsStringAsync();

        return Serializer.Deserialize<T>(serialized);
    }
}