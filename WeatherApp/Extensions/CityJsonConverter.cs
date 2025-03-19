using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using WeatherApp.Models;

namespace WeatherApp.Extensions
{
    class CityJsonConverter : JsonConverter<City>
    {
        public override City Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument doc = JsonDocument.ParseValue(ref reader);
            JsonElement root = doc.RootElement;

            var city = new City();

            if (root.TryGetProperty("local_names", out JsonElement local_names))
            {
                if (local_names.TryGetProperty("ru", out JsonElement ru))
                {
                    city.Name = ru.GetString();
                }
                else if (local_names.TryGetProperty("en", out JsonElement en))
                {
                    city.Name = en.GetString();
                }
            }
            if (root.TryGetProperty("name", out JsonElement name) && string.IsNullOrEmpty(city.Name))
            {
                city.Name = name.GetString();
            }

            if (root.TryGetProperty("lat", out JsonElement lat))
            {
                city.Latitude = lat.GetDouble();
            }
            if (root.TryGetProperty("lon", out JsonElement lon))
            {
                city.Longitude = lon.GetDouble();
            }
            if (root.TryGetProperty("country", out JsonElement country))
            {
                city.Country = country.GetString();
            }
                

            return city;
        }

        public override void Write(Utf8JsonWriter writer, City value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
