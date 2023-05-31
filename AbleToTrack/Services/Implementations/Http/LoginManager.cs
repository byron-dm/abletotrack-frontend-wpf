using System.Net;
using System.Net.Http;
using System.Text.Json;
using AbleToTrack.Model.Dtos.Requests;
using AbleToTrack.Model.Dtos.Responses;
using AbleToTrack.Model.Exceptions;
using AbleToTrack.Services.Definitions.Http;

namespace AbleToTrack.Services.Implementations.Http;

public class LoginManager : ILoginManager
{
    private readonly RequestManager _requestManager;

    public LoginManager(RequestManager requestManager)
    {
        _requestManager = requestManager;
    }
    
    public LoginResponseDto Login(LoginRequestDto loginRequest)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"{RequestManager.ApiUrl}login");
        var json = JsonSerializer.Serialize(loginRequest, _requestManager.SerializeOptions);
        request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = _requestManager.Client.Send(request);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return JsonSerializer.Deserialize<LoginResponseDto>(response.Content.ReadAsStream(), _requestManager.SerializeOptions);            
        }
        
        throw new HttpResponseException(response.StatusCode, response.ToString());
    }
}