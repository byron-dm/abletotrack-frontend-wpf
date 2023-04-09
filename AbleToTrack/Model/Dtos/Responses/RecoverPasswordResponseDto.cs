using System;

namespace AbleToTrack.Model.Dtos.Responses;

public record RecoverPasswordResponseDto(bool EmailSent = false, string Message = "", Exception? Exception = null);