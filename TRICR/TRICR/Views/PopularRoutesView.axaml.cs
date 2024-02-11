using System;
using TRICR.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace TRICR.Views;

public partial class PopularRoutesView : ReactiveUserControl<PopularRoutesViewModel>
{
    public PopularRoutesView()
    {
        this.WhenActivated(disposables => 
        {
            ViewModel.Refresh.Execute().Subscribe();
        });
        InitializeComponent();
    }
}