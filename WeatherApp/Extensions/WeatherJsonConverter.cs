using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Extensions
{
    public class WeatherJsonConverter : JsonConverter<Weather>
    {       
        public override Weather Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument doc = JsonDocument.ParseValue(ref reader);
            JsonElement root = doc.RootElement;

            var weather = new Weather();

            if (root.TryGetProperty("name", out JsonElement name))
            {
                weather.City.Name = name.GetString();
            }

            if (root.TryGetProperty("coord", out JsonElement coord))
            {
                if (coord.TryGetProperty("lon", out JsonElement lon))
                    weather.City.Longitude = lon.GetDouble();

                if (coord.TryGetProperty("lat", out JsonElement lat))
                    weather.City.Longitude = lat.GetDouble();
            }

            if (root.TryGetProperty("weather", out JsonElement weatherArray) && weatherArray.GetArrayLength() > 0)
            {
                var weatherInfo = weatherArray[0];
                if (weatherInfo.TryGetProperty("description", out JsonElement description))
                    weather.Description = description.GetString();

                if (weatherInfo.TryGetProperty("icon", out JsonElement icon))
                    weather.Icon = icon.GetString();
            }

            if (root.TryGetProperty("main", out JsonElement main))
            {
                if (main.TryGetProperty("temp", out JsonElement temp))
                    weather.Temperature = temp.GetDouble();
                if (main.TryGetProperty("feels_like", out JsonElement feelsLike))
                    weather.FeelsAsTemperature = feelsLike.GetDouble();
                if (main.TryGetProperty("humidity", out JsonElement humidity))
                    weather.Humidity = humidity.GetDouble();
                if (main.TryGetProperty("pressure", out JsonElement pressure))
                    weather.Pressure = pressure.GetDouble();
            }

            if (root.TryGetProperty("wind", out JsonElement wind))
            {
                if (wind.TryGetProperty("speed", out JsonElement speed))
                    weather.WindSpeed = speed.GetDouble();
                if (wind.TryGetProperty("deg", out JsonElement deg))
                    weather.WindDirection = deg.GetDouble();
            }

            if (root.TryGetProperty("clouds", out JsonElement clouds))
            {
                if (clouds.TryGetProperty("all", out JsonElement all))
                    weather.Cloudiness = all.GetDouble();
            }

            if (root.TryGetProperty("rain", out JsonElement rain))
            {
                if (rain.TryGetProperty("1h", out JsonElement oneHour))
                    weather.Rain = oneHour.GetDouble();
            }

            if (root.TryGetProperty("snow", out JsonElement snow))
            {
                if (snow.TryGetProperty("1h", out JsonElement oneHour))
                    weather.Snow = oneHour.GetDouble();
            }

            if (root.TryGetProperty("sys", out JsonElement sys))
            {
                if (sys.TryGetProperty("country", out JsonElement country))
                    weather.City.Country = country.GetString();
            }

            return weather;
        }

        public override void Write(Utf8JsonWriter writer, Weather value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
