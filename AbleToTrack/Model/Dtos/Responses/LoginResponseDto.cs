using System;

namespace AbleToTrack.Model.Dtos.Responses;

public record LoginResponseDto(bool Exists = false, long UserId = 0, bool IsEmailVerified = false,
    Exception? Exception = null);