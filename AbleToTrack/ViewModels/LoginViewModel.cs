using System.Security;
using System.Threading.Tasks;
using System.Windows;
using AbleToTrack.Events;
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

    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(CanLogin))] 
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string _email = "";

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(CanLogin))]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private SecureString? _password;

    [ObservableProperty]
    private bool? _shouldRememberMe = false;

    private readonly ILoginService _loginService;

    public LoginViewModel(ILoginService loginService)
    {
        _loginService = loginService;

        WeakReferenceMessenger.Default.Register<LoginVerificationFailed>(this, (r, m) => OnLoginVerificationFailed(m));
        WeakReferenceMessenger.Default.Register<LoginVerificationFinished>(this,
            (r, m) => OnLoginVerificationFinished(m));
    }

    public bool CanLogin => !(string.IsNullOrWhiteSpace(Email) || Password == null || Password.Length == 0);

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task LoginAsync()
    {
        await Task.Run(() => _loginService.Login(Email, Password, ShouldRememberMe));
    }

    private static void OnLoginVerificationFinished(LoginVerificationFinished theEvent)
    {
        if (theEvent.LoginResponse.Exists)
        {
            if (!theEvent.LoginResponse.IsEmailVerified)
            {
                MessageBox.Show(AppResources.ResourceManager.GetString("Login.Error.EmailNotVerified")!,
                    AppResources.ResourceManager.GetString("Login.MessageBox.Title.EmailNotVerified"),
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //WeakReferenceMessenger.Default.Send(new CloseApplicationRequested());
            return;
        }

        MessageBox.Show(AppResources.ResourceManager.GetString("Login.Error.InvalidUserOrPassword")!,
            AppResources.ResourceManager.GetString("Login.MessageBox.Title.LoginError"), MessageBoxButton.OK,
            MessageBoxImage.Error);
        //WeakReferenceMessenger.Default.Send(new AlertRequested(AppResources.ResourceManager.GetString("Login.Error.InvalidUserOrPassword")!));
    }

    private static void OnLoginVerificationFailed(LoginVerificationFailed theEvent)
    {
        MessageBox.Show(theEvent.ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}