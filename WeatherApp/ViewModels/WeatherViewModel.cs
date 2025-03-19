using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WeatherApp.Extensions;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.ViewModels
{
    public partial class WeatherViewModel : ViewModelBase
    {
        WeatherService _weatherService;
        private const string CityListKey = "CityList";
        [ObservableProperty]
        private ObservableCollection<City> cityList = new();
        [ObservableProperty]
        private ObservableCollection<Weather> weatherList = new();
        [ObservableProperty]
        private string cityName;

        public WeatherViewModel(WeatherService weatherService)
        {
            _weatherService = weatherService;

            var data = Preferences.Load(CityListKey, new List<City>()).Result;
            CityList = [..data];
            foreach(var city in cityList)
            {
                Forecast(city);
            }
        }

        [RelayCommand]
        public async void AddWeatherAsync(string cityName)
        {
            try
            {
                var zipcode = await _weatherService.GetGeoByCityNameAsync(cityName);
                if (zipcode == null)
                    return;
                await Forecast(zipcode);
                cityList.Add(zipcode);
                await SaveCity();
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private async Task Forecast(City zipcode)
        {
            var weather = await _weatherService.GetWeatherByGeoAsync(zipcode.Longitude, zipcode.Latitude);
            if (weather != null)
            {
                weather.City = zipcode;
                weather.Temperature = Math.Round(weather.Temperature - 273.1, 2);
                weather.FeelsAsTemperature = Math.Round(weather.FeelsAsTemperature - 273.1, 2);
                weather.Pressure = Math.Round(weather.Pressure / 133.3 * 100, 0);

                weatherList.Add(weather);
                
            }
        }

        [RelayCommand]
        private async Task SaveCity()
        {
            await Preferences.Save(CityListKey, CityList);

        }
    }
}
