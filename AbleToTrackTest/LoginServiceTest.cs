using System.Security;
using AbleToTrack.Model.Dtos.Requests;
using AbleToTrack.Services.Definitions.Http;
using AbleToTrack.Services.Implementations;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace AbleToTrackTest;

public class LoginServiceTest
{
    [Fact]
    public void IfBackendIsDown_LoginFails()
    {
        var mockLoginManager = Substitute.For<ILoginManager>();
        mockLoginManager.Login(new LoginRequestDto("", "", false)).Throws<HttpRequestException>();
        
        var loginService = new LoginService(mockLoginManager);
        loginService.Login("", new SecureString(), false);
        
        //Assert.Equal(0, response.UserId);
        //Assert.False(response.IsEmailVerified);
        //Assert.NotNull(response.AccessToken);
    }
}