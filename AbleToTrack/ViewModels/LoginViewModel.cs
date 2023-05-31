using System.Security;
using System.Threading.Tasks;
using AbleToTrack.Events.Dialogs;
using AbleToTrack.Events.Login;
using AbleToTrack.Resources;
using AbleToTrack.Services.Definitions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using log4net;

namespace AbleToTrack.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private static readonly ILog Logger =
        LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(CanLogin))] [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string _email = "";

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(CanLogin))] [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private SecureString? _password;

    [ObservableProperty] private bool _shouldRememberMe;

    private readonly ILoginService _loginService;

    public LoginViewModel(ILoginService loginService)
    {
        _loginService = loginService;

        WeakReferenceMessenger.Default.Register<BadCredentialsSent>(this, (_, _) => OnBadCredentialsSent());
        WeakReferenceMessenger.Default.Register<UnexpectedErrorReceived>(this,
            (_, theEvent) => OnUnexpectedErrorReceived(theEvent));
        WeakReferenceMessenger.Default.Register<UserVerificationFinished>(this,
            (_, theEvent) => OnEmailVerificationFinished(theEvent));
    }

    public bool CanLogin => !(string.IsNullOrWhiteSpace(Email) || Password == null || Password.Length == 0);

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task LoginAsync()
    {
        await Task.Run(() => _loginService.Login(Email, Password, ShouldRememberMe));
    }
    
    private void OnEmailVerificationFinished(UserVerificationFinished theEvent)
    {
        if (theEvent.IsVerified) return;
        
        WeakReferenceMessenger.Default.Send(new AlertRequested(
            AppResources.ResourceManager.GetString("Login.Error.EmailNotVerified"),
            AppResources.ResourceManager.GetString("Login.MessageBox.Title.LoginError"),
            AlertRequested.AlertButton.Ok, AlertRequested.AlertType.Error));
    }

    private void OnBadCredentialsSent()
    {
        WeakReferenceMessenger.Default.Send(new AlertRequested(
            AppResources.ResourceManager.GetString("Login.Error.InvalidUserOrPassword"),
            AppResources.ResourceManager.GetString("Login.MessageBox.Title.LoginError"),
            AlertRequested.AlertButton.Ok, AlertRequested.AlertType.Error));
    }

    private void OnUnexpectedErrorReceived(UnexpectedErrorReceived theEvent)
    {
        WeakReferenceMessenger.Default.Send(new AlertRequested(theEvent.ErrorMessage,
            AppResources.ResourceManager.GetString("Login.MessageBox.Title.LoginError"),
            AlertRequested.AlertButton.Ok, AlertRequested.AlertType.Error));
    }

    
}