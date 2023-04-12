using System.Security;
using AbleToTrack.Model.Dtos.Requests;
using AbleToTrack.Services.Definitions;
using AbleToTrack.Services.Implementations;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace AbleToTrackTest;

public class LoginServiceTest
{
    [Fact]
    public void IfBackendIsDown_LoginFails()
    {
        var mockUserManager = Substitute.For<IUserManager>();
        mockUserManager.Login(new LoginRequestDto("", "", false)).Throws<HttpRequestException>();
        
        var loginService = new LoginService(mockUserManager);
        var response = loginService.Login("", new SecureString(), false);
        
        Assert.False(response.Exists);
        Assert.Equal(0, response.UserId);
        Assert.False(response.IsEmailVerified);
        Assert.NotNull(response.Exception);
    }
    
    [Fact]
    public void IfBackendIsDown_RecoverPasswordFails()
    {
        var mockUserManager = Substitute.For<IUserManager>();
        mockUserManager.RecoverPassword(Arg.Any<string>()).Throws<HttpRequestException>();
        
        var loginService = new LoginService(mockUserManager);
        var response = loginService.RecoverPassword("");
        
        Assert.False(response.EmailSent);
        Assert.Empty(response.Message);
        Assert.NotNull(response.Exception);
    }
}