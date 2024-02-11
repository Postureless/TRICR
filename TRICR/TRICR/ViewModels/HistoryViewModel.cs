using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using TRICR.Controllers;
using TRICR.Models;
using TRICR.Services;
using ReactiveUI;

namespace TRICR.ViewModels;

public class HistoryViewModel : ReactiveObject, IRoutableViewModel
{
    public string? UrlPathSegment { get; }
    public IScreen HostScreen { get; }
    
    private DatabaseController _dbController;
    private ObservableCollection<SearchEntity> _searchHistory;
    public ReactiveCommand<Unit, IRoutableViewModel> GoBack { get; }
    public ObservableCollection<SearchEntity> SearchHistory
    {
        get => _searchHistory;
        set => this.RaiseAndSetIfChanged(ref _searchHistory, value);
    }

    public HistoryViewModel(RoutingState router, IScreen hostScreen, ApiService apiService, string location)
    {
        HostScreen = hostScreen;
        _dbController = new DatabaseController();
        LoadSearchHistory();
        
        GoBack = ReactiveCommand.CreateFromObservable(
            () =>
            {
                try
                {
                    return router.Navigate.Execute(new SearchViewModel(router, HostScreen, apiService, location));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return Observable.Return<IRoutableViewModel>(this);
                }
            },
            Observable.Return(true)
        );
    }

    private async void LoadSearchHistory()
    {
        var history = await _dbController.GetSearchQueriesAsync();
        SearchHistory = new ObservableCollection<SearchEntity>(history);
    }
}