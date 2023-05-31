using System.Net.Http;
using System.Threading.Tasks;
using AbleToTrack.Events.Dialogs;
using AbleToTrack.Events.Login;
using AbleToTrack.Model.Dtos.Responses;
using AbleToTrack.Services.Definitions;
using AbleToTrack.Services.Definitions.Http;
using CommunityToolkit.Mvvm.Messaging;

namespace AbleToTrack.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserManager _userManager;

    public UserService(IUserManager userManager)
    {
        _userManager = userManager;
        
        WeakReferenceMessenger.Default.Register<UserVerificationFinished>(this,
            (_, theEvent) => OnUserVerificationFinished(theEvent));
    }
    
    public RecoverPasswordResponseDto RecoverPassword(string email)
    {
        try
        {
            return _userManager.RecoverPassword(email);
        }
        catch (HttpRequestException exception)
        {
            return new RecoverPasswordResponseDto(Exception: exception);
        }
    }
    
    private async void OnUserVerificationFinished(UserVerificationFinished theEvent)
    {
        if (!theEvent.IsVerified) return;
        
        var userProfile = await Task.Run(() => _userManager.GetProfile(theEvent.UserId));
        
        WeakReferenceMessenger.Default.Send(new MainWindowRequested(userProfile));
    }
}