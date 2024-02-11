using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using TRICR.Services;
using ReactiveUI;

namespace TRICR.ViewModels;

public class ResponseViewModel : ReactiveObject, IRoutableViewModel
{
    public string? UrlPathSegment { get; } = "response";
    public IScreen HostScreen { get; }
    
    private readonly ApiService _apiService;
    
    public ReactiveCommand<Unit, IRoutableViewModel> GoBack { get; }
    public ReactiveCommand<Unit, Unit> Refresh { get; }
    
    private ObservableCollection<DirectionDetails> _directionsData = new ObservableCollection<DirectionDetails>();

    public ObservableCollection<DirectionDetails> DirectionsData
    {
        get => _directionsData;
        set => this.RaiseAndSetIfChanged(ref _directionsData, value);
    }
    
    public ResponseViewModel(RoutingState router, IScreen hostScreen, ApiService apiService, string origin, string destination, string departDate, string returnDate, string location)
    {
        HostScreen = hostScreen;
        _apiService = apiService;
        
        GoBack = ReactiveCommand.CreateFromObservable(
            () =>
            {
                try
                {
                    return router.Navigate.Execute(new SearchViewModel(router, HostScreen, _apiService, location));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return Observable.Return<IRoutableViewModel>(this);
                }
            },
            Observable.Return(true)
        );

        Refresh = ReactiveCommand.CreateFromTask(
            async () =>
            {
                try
                {
                    var data = await _apiService.GetCalendarPricesData(origin, destination, departDate, returnDate, "USD", "f19f8d8de5e05bc4726ebf898a67a266");
                    if (data != null)
                    {
                        DirectionsData = new ObservableCollection<DirectionDetails>(data.Data.Values);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error fetching data: {e.Message}");
                }
            }
        );
    }
}