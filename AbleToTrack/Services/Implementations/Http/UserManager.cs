using System.Net;
using System.Net.Http;
using System.Text.Json;
using AbleToTrack.Model.Dtos.Responses;
using AbleToTrack.Model.Exceptions;
using AbleToTrack.Services.Definitions.Http;

namespace AbleToTrack.Services.Implementations.Http;

public class UserManager : IUserManager
{
    private readonly RequestManager _requestManager;
    
    public UserManager(RequestManager requestManager)
    {
        _requestManager = requestManager;
    }
    
    public UserResponseDto GetProfile(long userId)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"{RequestManager.ApiUrl}user/get-profile");
        var json = JsonSerializer.Serialize(userId, _requestManager.SerializeOptions);
        request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        
        var response = _requestManager.Client.Send(request);
        
        if (response.StatusCode == HttpStatusCode.OK)
        {
            return JsonSerializer.Deserialize<UserResponseDto>(response.Content.ReadAsStream(), _requestManager.SerializeOptions);            
        }

        throw new HttpResponseException(response.StatusCode, response.ToString());
    }
    
    public RecoverPasswordResponseDto RecoverPassword(string email)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"{RequestManager.ApiUrl}user/recover-password");
        var json = JsonSerializer.Serialize(email, _requestManager.SerializeOptions);
        request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = _requestManager.Client.Send(request);

        return JsonSerializer.Deserialize<RecoverPasswordResponseDto>(response.Content.ReadAsStream(), _requestManager.SerializeOptions);
    }
}