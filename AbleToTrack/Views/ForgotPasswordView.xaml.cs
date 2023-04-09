using System.Windows.Input;
using AbleToTrack.Events.Dialogs;
using AbleToTrack.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace AbleToTrack.Views;

public partial class ForgotPasswordView
{
    public ForgotPasswordView()
    {
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<ForgotPasswordRequested>(this, (_, _) => OnForgotPasswordRequested());
    }

    private void OnForgotPasswordRequested()
    {
        TextBoxEmail.Focus();
        Show();
    }

    private void IconClose_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        Hide();
    }

    private void TextBoxEmail_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Return && ((ForgotPasswordViewModel)DataContext).CanSendEmail)
        {
            ButtonSendEmail.Command.Execute(null);
        }
    }
}    