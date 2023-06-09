﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AbleToTrack.Events.Dialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace AbleToTrack.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly BitmapImage _defaultPhoto = new(
        new Uri(@"/AbleToTrack;component/Resources/Images/user-picture.png", UriKind.Relative));

    [ObservableProperty] private string _userName = "";

    [ObservableProperty] private ImageSource _userPhoto;

    public MainViewModel()
    {
        _userPhoto = _defaultPhoto;

        WeakReferenceMessenger.Default.Register<MainWindowRequested>(this,
            (_, theEvent) => OnMainWindowRequested(theEvent));
    }

    private void OnMainWindowRequested(MainWindowRequested theEvent)
    {
        UserName = $"{theEvent.User.FirstName} {theEvent.User.LastName}";
        
        if (string.IsNullOrEmpty(theEvent.User.Picture)) return;

        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(Convert.FromBase64String(theEvent.User.Picture));
            bitmapImage.EndInit();
            
            UserPhoto = bitmapImage;
        }));
    }
}