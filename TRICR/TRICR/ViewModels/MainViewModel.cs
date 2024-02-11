using System;
using System.Reactive;
using TRICR.Services;
using ReactiveUI;

namespace TRICR.ViewModels;

public class MainViewModel : ReactiveObject, IScreen
{
    public RoutingState Router { get; } = new();

    public MainViewModel()
    {
        var screen = new LocationViewModel(Router, this, new ApiService());
        Router.Navigate.Execute(screen);
    }
}