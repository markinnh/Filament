﻿using System;

using Filament.UWP.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Filament.UWP.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
