using AbleToTrack.Model.Dtos.Requests;
using AbleToTrack.Model.Dtos.Responses;

namespace AbleToTrack.Services.Definitions.Http;

public interface ILoginManager
{
    LoginResponseDto Login(LoginRequestDto loginRequest);
}