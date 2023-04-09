using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AbleToTrack.Services.Implementations;

public abstract class RequestManager
{
    protected readonly HttpClient Client = new();
    protected readonly JsonSerializerOptions SerializeOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    protected const string ApiUrl = "http://localhost:8080/";
    
    protected RequestManager()
    {
        Client.DefaultRequestHeaders.Accept.Clear();
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
}