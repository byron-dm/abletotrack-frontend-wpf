using System;

namespace AbleToTrack.Model.Dtos.Responses;

public record LoginResponseDto(string AccessToken = "", long UserId = 0, bool IsEmailVerified = false);