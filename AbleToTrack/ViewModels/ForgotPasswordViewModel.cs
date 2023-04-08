using System.Threading.Tasks;
using System.Windows;
using AbleToTrack.Resources;
using AbleToTrack.Services.Definitions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AbleToTrack.ViewModels;

public partial class ForgotPasswordViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanSendEmail))]
    [NotifyCanExecuteChangedFor(nameof(SendMailCommand))]
    private string _email = "";

    [ObservableProperty] private string _errorMessage = "";

    private readonly ILoginService _loginService;

    public ForgotPasswordViewModel(ILoginService loginService)
    {
        _loginService = loginService;
    }

    public bool CanSendEmail => !string.IsNullOrWhiteSpace(Email);

    [RelayCommand(CanExecute = nameof(CanSendEmail))]
    private async Task SendMailAsync()
    {
        ErrorMessage = "";

        var response = await Task.Run(() => _loginService.RecoverPassword(Email));
        if (response.EmailSent)
        {
            MessageBox.Show(response.Message, AppResources.ResourceManager.GetString("ForgotPassword.TextBlock.Title"),
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            ErrorMessage = response.Message;
        }
    }
}