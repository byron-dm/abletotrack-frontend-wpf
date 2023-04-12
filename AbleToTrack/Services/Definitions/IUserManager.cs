﻿using AbleToTrack.Model.Dtos.Requests;
using AbleToTrack.Model.Dtos.Responses;

namespace AbleToTrack.Services.Definitions;

public interface IUserManager
{
    UserResponseDto GetUser(long userId);
    
    LoginResponseDto Login(LoginRequestDto loginRequest);

    RecoverPasswordResponseDto RecoverPassword(string email);
}