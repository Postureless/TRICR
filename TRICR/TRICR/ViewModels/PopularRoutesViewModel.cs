using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using TRICR.Services;
using ReactiveUI;

namespace TRICR.ViewModels
{
    public class PopularRoutesViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly ApiService _apiService;

        public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
        private string _location;

        public string Location
        {
            get => _location;
            set => this.RaiseAndSetIfChanged(ref _location, value);
        }

        public IScreen HostScreen { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoBack { get; }
        public ReactiveCommand<Unit, Unit> Refresh { get; }

        private ObservableCollection<DirectionDetails> _directionsData = new ObservableCollection<DirectionDetails>();

        public ObservableCollection<DirectionDetails> DirectionsData
        {
            get => _directionsData;
            set => this.RaiseAndSetIfChanged(ref _directionsData, value);
        }

        public PopularRoutesViewModel(RoutingState router, IScreen hostScreen, ApiService apiService, string location)
        {
            HostScreen = hostScreen;
            _apiService = apiService;
            Location = location;

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
                        var data = await _apiService.GetPopularDirectionsData(Location, "usd", "f19f8d8de5e05bc4726ebf898a67a266");
                        if (data != null)
                        {
                            DirectionsData = new ObservableCollection<DirectionDetails>(data.Data.Values);
                        }
                        else
                        {
                            Console.WriteLine("Empty data");
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
}
