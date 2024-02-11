using System;
using Avalonia.ReactiveUI;
using ReactiveUI;
using TRICR.ViewModels;
using Avalonia.Markup.Xaml;

namespace TRICR.Views;

public partial class LocationView : ReactiveUserControl<LocationViewModel>
{
    public LocationView()
    {
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }
}