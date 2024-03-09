using Newtonsoft.Json;
using RestSharp;

public class ApiHelper : IDisposable
{
    private readonly RestClient _client;

    public ApiHelper()
    {
        _client = new RestClient("https://jsonplaceholder.typicode.com");
    }

    public RestResponse Get(string endpoint)
    {
        var request = new RestRequest(endpoint, Method.Get);
        var response = _client.Execute(request);
        return response;
    }

    public RestResponse Post(string endpoint, object body)
    {
        var request = new RestRequest(endpoint, Method.Post);
        request.AddJsonBody(body);
        var response = _client.Execute(request);
        return response;
    }

    public RestResponse Put(string endpoint, object body)
    {
        var request = new RestRequest(endpoint, Method.Put);
        request.AddJsonBody(body); // Assuming JSON payload
        return _client.Execute(request);
    }

    public RestResponse Delete(string endpoint)
    {
        var request = new RestRequest(endpoint, Method.Delete);
        return _client.Execute(request);
    }

    public T DeserializeResponse<T>(RestResponse response)
    {
        var content = response.Content;

        if (content == null)
        {
            throw new ArgumentNullException(nameof(content), "Response content is null.");
        }

        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        })!;
    }

    public void Dispose()
    {
        
    }
}
