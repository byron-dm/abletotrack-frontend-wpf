using System.Collections.Generic;
using System.Windows;
using AbleToTrack.Events.Dialogs;
using CommunityToolkit.Mvvm.Messaging;

namespace AbleToTrack.Views;

public partial class AlertView
{
    private readonly Dictionary<AlertRequested.AlertButton, MessageBoxButton> _alertButtons = new ();
    private readonly Dictionary<AlertRequested.AlertType, MessageBoxImage> _alertTypes = new ();

    public AlertView()
    {
        _alertButtons.Add(AlertRequested.AlertButton.Ok, MessageBoxButton.OK);
        _alertTypes.Add(AlertRequested.AlertType.Error, MessageBoxImage.Error);
        _alertTypes.Add(AlertRequested.AlertType.Information, MessageBoxImage.Information);
        
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<AlertRequested>(this, (_, m) => OnAlertRequested(m));
    }

    private void OnAlertRequested(AlertRequested theEvent)
    {
        Dispatcher.Invoke(() =>
        {
            //TextBlockMessage.Text = theEvent.Message;
            //Show();
            MessageBox.Show(theEvent.Message, theEvent.Title,
                _alertButtons[theEvent.Button], _alertTypes[theEvent.Type]);
        });
    }

    private void ButtonClose_OnClick(object sender, RoutedEventArgs e)
    {
        //Dispatcher.Invoke(Close);
    }
}