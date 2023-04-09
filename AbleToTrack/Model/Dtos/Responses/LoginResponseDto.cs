using System;

namespace AbleToTrack.Model.Dtos.Responses;

public record LoginResponseDto(bool Exists = false, string FirstName = "", string LastName = "",
    bool IsEmailVerified = false, Exception? Exception = null);