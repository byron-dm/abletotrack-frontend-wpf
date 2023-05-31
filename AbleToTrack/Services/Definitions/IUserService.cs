using AbleToTrack.Model.Dtos.Responses;

namespace AbleToTrack.Services.Definitions;

public interface IUserService
{
    RecoverPasswordResponseDto RecoverPassword(string email);
}