using System.Net;
using System.Security;
using AbleToTrack.Events.Login;
using AbleToTrack.Model.Dtos.Requests;
using AbleToTrack.Model.Exceptions;
using AbleToTrack.Services.Definitions;
using AbleToTrack.Services.Definitions.Http;
using CommunityToolkit.Mvvm.Messaging;
using log4net;

namespace AbleToTrack.Services.Implementations;

public class LoginService : ILoginService
{
    private static readonly ILog Logger =
        LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
    private readonly ILoginManager _loginManager;

    public LoginService(ILoginManager loginManager)
    {
        _loginManager = loginManager;
    }
    
    public void Login(string email, SecureString password, bool shouldRememberMe)
    {
        var decodedPassword = new NetworkCredential(string.Empty, password).Password;
        Logger.Info(decodedPassword);

        try
        {
            var response = _loginManager.Login(new LoginRequestDto(email, decodedPassword, shouldRememberMe));

            WeakReferenceMessenger.Default.Send(new UserVerificationFinished(response.AccessToken, response.UserId, response.IsEmailVerified));
        }
        catch (HttpResponseException exception)
        {
            if (exception.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                WeakReferenceMessenger.Default.Send(new BadCredentialsSent());
            }
            else
            {
                WeakReferenceMessenger.Default.Send(new UnexpectedErrorReceived(exception.Message));
            }
        }
    }
}