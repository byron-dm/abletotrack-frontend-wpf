namespace AbleToTrack.Events.Login;

public record UserVerificationFinished(string AccessToken, long UserId, bool IsVerified);