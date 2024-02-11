using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using TRICR.Controllers;
using TRICR.Models;
using TRICR.Services;
using Microsoft.CodeAnalysis;
using ReactiveUI;

namespace TRICR.ViewModels;

public class SearchViewModel : ReactiveObject, IRoutableViewModel
{
    public string? UrlPathSegment { get; } = "search";
    public IScreen HostScreen { get; }
    
    private readonly ApiService _apiService;
    private string _location;
    private DatabaseController _dbController;
    public ReactiveCommand<Unit, IRoutableViewModel> Popular { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> History { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> Response { get; }

    private string _from;
    public string From
    {
        get => _from;
        set => this.RaiseAndSetIfChanged(ref _from, value);
    }
    
    private string _where;
    public string Where
    {
        get => _where;
        set => this.RaiseAndSetIfChanged(ref _where, value);
    }
    
    private string _date;
    public string Date
    {
        get => _date;
        set
        {
            this.RaiseAndSetIfChanged(ref _date, value);
            ProcessDateInput();
        }
    }

    public string DepartureDate { get; private set; }
    public string ReturnDate { get; private set; }
    
    private string _passengers;
    public string Passengers
    {
        get => _passengers;
        set => this.RaiseAndSetIfChanged(ref _passengers, value);
    }
    
    public SearchViewModel(RoutingState router, IScreen hostScreen, ApiService apiService, string location)
    {
        HostScreen = hostScreen;
        _apiService = apiService;
        _dbController = new DatabaseController();
        
        Popular = ReactiveCommand.CreateFromObservable(
            () =>
            {
                try
                {
                    return router.Navigate.Execute(
                        new PopularRoutesViewModel(router, HostScreen, _apiService, location));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return Observable.Return<IRoutableViewModel>(this);
                }
            });

        Response = ReactiveCommand.CreateFromObservable(
            () =>
            {
                try
                {
                    SaveSearchQuery();
                    return router.Navigate.Execute(
                        new ResponseViewModel(router, HostScreen, _apiService, From, Where, DepartureDate, ReturnDate, location));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return Observable.Return<IRoutableViewModel>(this);
                }
            });
        
        History = ReactiveCommand.CreateFromObservable(
            () =>
            {
                try
                {
                    return router.Navigate.Execute(new HistoryViewModel(router, HostScreen, _apiService, location));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return Observable.Return<IRoutableViewModel>(this);
                }
            });
    }
    
    private void ProcessDateInput()
    {
        var dates = _date?.Split(new[] { " - " }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .ToList();

        if (dates != null && dates.Count == 2 && 
            DateTime.TryParse(dates[0], out var departureDate) && 
            DateTime.TryParse(dates[1], out var returnDate))
        {
            DepartureDate = departureDate.ToString("yyyy-MM-dd");
            ReturnDate = returnDate.ToString("yyyy-MM-dd");
        }
        else
        {
            Console.WriteLine("Wrong input format");
        }
    }
    
    private async void SaveSearchQuery()
    {
        var newQuery = new SearchEntity()
        {
            From = this.From,
            Where = this.Where,
            DepartureDate = this.DepartureDate,
            ReturnDate = this.ReturnDate,
            Passengers = this.Passengers
        };
        
        Console.WriteLine(newQuery.From + newQuery.Where + newQuery.ReturnDate + newQuery.DepartureDate + newQuery.Passengers + newQuery.ID);

        await _dbController.SaveSearchQueryAsync(newQuery);
    }
}