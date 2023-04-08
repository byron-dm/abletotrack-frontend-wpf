using System.Windows;
using AbleToTrack.Events;
using CommunityToolkit.Mvvm.Messaging;

namespace AbleToTrack.Views;

public partial class AlertView
{
    public AlertView()
    {
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<AlertRequested>(this, (_, m) => OnAlertRequested(m));
    }

    private void OnAlertRequested(AlertRequested theEvent)
    {
        Dispatcher.Invoke(() =>
        {
            TextBlockMessage.Text = theEvent.Message;
            Show();
        });
    }

    private void ButtonClose_OnClick(object sender, RoutedEventArgs e)
    {
        Dispatcher.Invoke(Close);
    }
}