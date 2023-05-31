using System;
using System.Windows;
using System.Windows.Input;
using AbleToTrack.Events.Dialogs;
using CommunityToolkit.Mvvm.Messaging;

namespace AbleToTrack.Views;

public partial class MainView
{
    public MainView()
    {
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<MainWindowRequested>(this, (_, _) => OnMainWindowRequested()); 
    }

    private void MainWindowView_OnMouseMove(object sender, MouseEventArgs e)
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

    private void OnMainWindowRequested()
    {
        Dispatcher.BeginInvoke(new Action(() =>
        {
            Show();
            Application.Current.Windows[0]?.Hide();
        }));
    }
}