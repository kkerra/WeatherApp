using Avalonia.Media.Imaging;
using System;
using System.Threading.Tasks;
using WeatherApp.Extensions;

namespace WeatherApp.Models
{
    public class Weather
    {
        public City City { get; set; } = new City();
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public Task<Bitmap?> IconBitmap { get=> ImageHelper.LoadFromWeb(new Uri($"http://openweathermap.org/img/wn/{Icon}@2x.png")); } 
        
        public double Temperature { get; set; }
        public double FeelsAsTemperature { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
        public double WindSpeed { get; set; }
        public double WindDirection { get; set; }
        public double Cloudiness { get; set; }
        public double Rain { get; set; }
        public double Snow { get; set; }
        public DateTime DateTime { get; set; }
    }
}

