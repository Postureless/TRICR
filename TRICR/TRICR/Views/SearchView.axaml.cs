using Avalonia.ReactiveUI;
using ReactiveUI;
using TRICR.ViewModels;

namespace TRICR.Views;

public partial class SearchView : ReactiveUserControl<SearchViewModel>
{
    public SearchView()
    {
        this.WhenActivated(_ => { });
        InitializeComponent();
    }
}