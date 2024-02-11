using System;
using TRICR.ViewModels;
using TRICR.Views;
using ReactiveUI;

namespace TRICR;

public class AppViewLocator : IViewLocator
{
    public IViewFor ResolveView<T>(T viewModel, string contract = null) => viewModel switch
    {
        MainViewModel mainViewModel => new MainView() { DataContext = mainViewModel},
        LocationViewModel context => new LocationView() { DataContext = context },
        SearchViewModel context => new SearchView() { DataContext = context },
        PopularRoutesViewModel context => new PopularRoutesView() {DataContext = context },
        ResponseViewModel context => new Response() {DataContext = context},
        HistoryViewModel context => new HistoryView() {DataContext = context},
        _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
    };
}