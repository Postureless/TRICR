using TRICR.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace TRICR.Views;

public partial class HistoryView : ReactiveUserControl<HistoryViewModel>
{
    public HistoryView()
    {
        InitializeComponent();
    }
}