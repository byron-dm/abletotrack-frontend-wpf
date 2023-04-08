using System.Net;

namespace AbleToTrack.Events;

public record LoginVerificationFailed(HttpStatusCode? HttpErrorCode, string ErrorMessage);