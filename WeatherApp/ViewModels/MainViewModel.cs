using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    ViewModelBase currentPage = App.Services.GetRequiredService<WeatherViewModel>();
}
