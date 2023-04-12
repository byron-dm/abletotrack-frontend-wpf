namespace AbleToTrack.Model.Dtos.Responses;

public record UserResponseDto(string FirstName, string LastName, UserProfileResponseDto Profile);