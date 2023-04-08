using System.Windows;
using System.Windows.Input;
using AbleToTrack.Events;
using CommunityToolkit.Mvvm.Messaging;

namespace AbleToTrack.Views;

public partial class LoginView
{
    public LoginView()
    {
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<CloseApplicationRequested>(this, (_, _) => OnCloseApplicationRequested()); }

    private void LoginView_OnMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }
    
    private void IconMinimize_OnMouseDown(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void IconClose_OnMouseDown(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void TextBoxEmail_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Return && !string.IsNullOrEmpty(TextBoxEmail.Text))
        {
            BindablePasswordBoxPassword.BindablePassword.Focus();
        }
    }

    private void BindablePasswordBoxPassword_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Return && !string.IsNullOrEmpty(TextBoxEmail.Text) 
                                && !string.IsNullOrEmpty(BindablePasswordBoxPassword.BindablePassword.Password))
        {
            ButtonLogin.Command.Execute(null);
        }
    }
    
    private void OnCloseApplicationRequested()
    {
        Close();
    }

    private void TextBlockForgotPassword_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        WeakReferenceMessenger.Default.Send(new ForgotPasswordRequested());
    }
}