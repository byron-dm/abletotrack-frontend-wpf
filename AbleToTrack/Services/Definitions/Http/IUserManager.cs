using AbleToTrack.Model.Dtos.Responses;

namespace AbleToTrack.Services.Definitions.Http;

public interface IUserManager
{
    UserResponseDto GetProfile(long userId);

    RecoverPasswordResponseDto RecoverPassword(string email);
}