using System;
using TRICR.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace TRICR.Views;

public partial class Response : ReactiveUserControl<ResponseViewModel>
{
    public Response()
    {
        this.WhenActivated(disposables => 
        {
            ViewModel.Refresh.Execute().Subscribe();
        });
        InitializeComponent();
    }
}