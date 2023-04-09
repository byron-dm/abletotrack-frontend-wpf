using System.Net.Http;
using System.Text.Json;
using AbleToTrack.Model.Dtos.Requests;
using AbleToTrack.Model.Dtos.Responses;
using AbleToTrack.Services.Definitions;

namespace AbleToTrack.Services.Implementations;

public class UserManager : RequestManager, IUserManager
{
    public LoginResponseDto Login(LoginRequestDto loginRequest)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"{ApiUrl}login");
        var json = JsonSerializer.Serialize(loginRequest, SerializeOptions);
        request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = Client.Send(request);

        return JsonSerializer.Deserialize<LoginResponseDto>(response.Content.ReadAsStream(), SerializeOptions);
    }

    public RecoverPasswordResponseDto RecoverPassword(string email)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"{ApiUrl}recover-password");
        var json = JsonSerializer.Serialize(email, SerializeOptions);
        request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = Client.Send(request);

        return JsonSerializer.Deserialize<RecoverPasswordResponseDto>(response.Content.ReadAsStream(), SerializeOptions);
    }
}