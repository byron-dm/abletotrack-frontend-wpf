namespace AbleToTrack.Model.Dtos.Responses;

public record LoginResponseDto(bool Exists, string FirstName, string LastName, bool IsEmailVerified);