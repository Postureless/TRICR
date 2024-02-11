using System;
using TRICR.Services;
using Avalonia.Controls;
using TRICR.ViewModels;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace TRICR.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
    }
}