using System;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using TRICR.ViewModels;

namespace TRICR.Views;

public partial class MainView : ReactiveUserControl<MainViewModel>
{
    public MainView()
    {
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }
}