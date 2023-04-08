namespace AbleToTrack.Model.Dtos.Requests;

public record LoginRequestDto(string Email, string Password, bool ShouldRememberMe);