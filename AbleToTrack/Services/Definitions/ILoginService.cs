﻿using System.Security;
using AbleToTrack.Model.Dtos.Responses;

namespace AbleToTrack.Services.Definitions;

public interface ILoginService
{
    LoginResponseDto Login(string email, SecureString password, bool shouldRememberMe);

    RecoverPasswordResponseDto RecoverPassword(string email);
}