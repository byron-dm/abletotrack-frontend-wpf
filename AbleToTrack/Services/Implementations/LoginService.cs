using System.Net;
using System.Net.Http;
using System.Security;
using AbleToTrack.Events;
using AbleToTrack.Model.Dtos.Requests;
using AbleToTrack.Model.Dtos.Responses;
using AbleToTrack.Services.Definitions;
using AbleToTrack.Services.Rest;
using CommunityToolkit.Mvvm.Messaging;

namespace AbleToTrack.Services.Implementations;

public class LoginService : ILoginService
{
    private readonly UserManager _userManager;

    public LoginService(UserManager userManager)
    {
        _userManager = userManager;
    }

    public void Login(string email, SecureString password, bool? shouldRememberMe)
    {
        var decodedPassword = new NetworkCredential(string.Empty, password).Password;

        try
        {
            var loginResponse = _userManager.Login(new LoginRequestDto(email, decodedPassword, shouldRememberMe ?? false));
            WeakReferenceMessenger.Default.Send(new LoginVerificationFinished(loginResponse));
        }
        catch (HttpRequestException exception)
        {
            WeakReferenceMessenger.Default.Send(new LoginVerificationFailed(exception.StatusCode, exception.Message));
        }
    }

    public RecoverPasswordResponseDto RecoverPassword(string email)
    {
        try
        {
            return _userManager.RecoverPassword(email);
            //WeakReferenceMessenger.Default.Send(new LoginVerificationFinished(loginResponse));
        }
        catch (HttpRequestException exception)
        {
            return new RecoverPasswordResponseDto(false, exception.Message);
            //WeakReferenceMessenger.Default.Send(new LoginVerificationFailed(exception.StatusCode, exception.Message));
        }
    }
}