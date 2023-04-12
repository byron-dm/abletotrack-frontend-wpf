using System.Net;
using System.Net.Http;
using System.Security;
using AbleToTrack.Model.Dtos.Requests;
using AbleToTrack.Model.Dtos.Responses;
using AbleToTrack.Services.Definitions;

namespace AbleToTrack.Services.Implementations;

public class LoginService : ILoginService
{
    private readonly IUserManager _userManager;

    public LoginService(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public UserResponseDto GetUser(long userId)
    {
        return _userManager.GetUser(userId);
    }

    public LoginResponseDto Login(string email, SecureString password, bool shouldRememberMe)
    {
        var decodedPassword = new NetworkCredential(string.Empty, password).Password;

        try
        {
            return _userManager.Login(new LoginRequestDto(email, decodedPassword, shouldRememberMe));
        }
        catch (HttpRequestException exception)
        {
            return new LoginResponseDto(Exception: exception);
        }
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
}