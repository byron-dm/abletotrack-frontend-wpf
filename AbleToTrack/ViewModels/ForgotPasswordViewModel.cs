using System.Threading.Tasks;
using AbleToTrack.Events.Dialogs;
using AbleToTrack.Resources;
using AbleToTrack.Services.Definitions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AbleToTrack.ViewModels;

public partial class ForgotPasswordViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSendEmail))]
    [NotifyCanExecuteChangedFor(nameof(SendMailCommand))]
    private string _email = "";

    [ObservableProperty] private string _errorMessage = "";

    private readonly IUserService _userService;

    public ForgotPasswordViewModel(IUserService userService)
    {
        _userService = userService;
        WeakReferenceMessenger.Default.Register<ForgotPasswordRequested>(this, (_, _) => OnForgotPasswordRequested());
    }

    public bool CanSendEmail => !string.IsNullOrWhiteSpace(Email);

    [RelayCommand(CanExecute = nameof(CanSendEmail))]
    private async Task SendMailAsync()
    {
        ErrorMessage = "";
        
        var response = await Task.Run(() => _userService.RecoverPassword(Email));
        string message;
        
        if (response.Exception == null)
        {
            message = response.Message;
            if (response.EmailSent)
            {
                WeakReferenceMessenger.Default.Send(new AlertRequested(message,
                    AppResources.ResourceManager.GetString("ForgotPassword.TextBlock.Title")));
                WeakReferenceMessenger.Default.Send(new CloseCurrentWindowRequested());
            }
        }
        else
        {
            message = response.Exception.Message;
        }

        ErrorMessage = message;
    }
    
    private void OnForgotPasswordRequested()
    {
        Email = "";
        ErrorMessage = "";
    }
}