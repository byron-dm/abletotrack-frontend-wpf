using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using AbleToTrack.Events.Login;
using CommunityToolkit.Mvvm.Messaging;

namespace AbleToTrack.Services.Implementations.Http;

public class RequestManager
{
    public static string ApiUrl => "http://localhost:8080/";
    public readonly HttpClient Client = new();
    public readonly JsonSerializerOptions SerializeOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public RequestManager()
    {
        Client.DefaultRequestHeaders.Accept.Clear();
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        
        WeakReferenceMessenger.Default.Register<UserVerificationFinished>(this,
            (_, theEvent) => OnUserVerificationFinished(theEvent));
    }
    
    private void OnUserVerificationFinished(UserVerificationFinished theEvent)
    {
        if (!theEvent.IsVerified) return;
        
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", theEvent.AccessToken);
    }
}