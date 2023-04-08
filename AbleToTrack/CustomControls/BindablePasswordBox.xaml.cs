﻿using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace AbleToTrack.CustomControls;

public partial class BindablePasswordBox : UserControl
{
    public static readonly DependencyProperty PasswordProperty = 
        DependencyProperty.Register("Password", typeof(SecureString), typeof(BindablePasswordBox));

    public BindablePasswordBox()
    {
        InitializeComponent();
        BindablePassword.PasswordChanged += OnPasswordChanged;
    }
    
    public SecureString Password
    {
        get => (SecureString)GetValue(PasswordProperty);
        set => SetValue(PasswordProperty, value);
    }
    
    
    private void OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        Password = BindablePassword.SecurePassword;
    }
}