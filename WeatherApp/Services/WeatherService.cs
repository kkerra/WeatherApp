using Avalonia.Markup.Xaml.MarkupExtensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherApp.Extensions;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class WeatherService
    {
        private HttpClient _httpClient;
        private readonly string _apiKey;

        public WeatherService()
        {
            _httpClient = new HttpClient();
            var key = App.Configuration.GetSection("ApiKeys")["WeatherApi"];
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("Weather API key not found in appsettings.json");
            }
            _apiKey = key;
        }

        public async Task<Weather> GetWeatherByCityNameAsync(string cityName)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                options.Converters.Add(new WeatherJsonConverter());

                var url = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={_apiKey}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStreamAsync();
                var weatherData = await JsonSerializer.DeserializeAsync<Weather>(json, options);
                return weatherData;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"An error occurred while downloading image: {ex.Message}");
                return null;
            }
        }

        public async Task<Weather> GetWeatherByGeoAsync(double longitude, double latitude)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                options.Converters.Add(new WeatherJsonConverter());
                var url = $"http://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&limit=1&appid={_apiKey}&lang=ru";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStreamAsync();
                var weatherData = await JsonSerializer.DeserializeAsync<Weather>(json, options);
                return weatherData;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"An error occurred while downloading image: {ex.Message}");
                return null;
            }
        }

        public async Task<City> GetGeoByCityNameAsync(string cityName)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                options.Converters.Add(new CityJsonConverter());
                var url = $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&appid={_apiKey}&lang=ru";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStreamAsync();
                var geoCity = await JsonSerializer.DeserializeAsync<List<City>>(json, options);
                return geoCity[0];
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"An error occurred while downloading image: {ex.Message}");
                return null;
            }
        }
    }
}

