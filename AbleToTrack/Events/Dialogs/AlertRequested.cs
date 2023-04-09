namespace AbleToTrack.Events.Dialogs;

public class AlertRequested
{
    public enum AlertButton
    {
        Ok
    }

    public enum AlertType
    {
        Error,
        Information
    }

    public AlertRequested(string? message = "", string? title = "", AlertButton button = AlertButton.Ok,
        AlertType type = AlertType.Information)
    {
        Button = button;
        Type = type;
        Message = message ?? "";
        Title = title ?? "";
    }

    public AlertButton Button { get; }
    public AlertType Type { get; }
    public string Message { get; }
    public string Title { get; }
}