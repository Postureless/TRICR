using System;
using System.Reactive;
using TRICR.Services;
using ReactiveUI;

namespace TRICR.ViewModels;

public class MainWindowViewModel : ReactiveObject, IScreen
{
    public RoutingState Router { get; } = new RoutingState();
    public ReactiveCommand<Unit, IRoutableViewModel> GoNext { get; }
    

    public MainWindowViewModel()
    {
        Router.Navigate.Execute(new LocationViewModel(Router, this, new ApiService()));
    }
}