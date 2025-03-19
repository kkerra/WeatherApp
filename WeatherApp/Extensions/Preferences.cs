using Avalonia.Platform.Storage;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using Avalonia.Controls;

namespace WeatherApp.Extensions
{
    public class Preferences
    {
        private static async Task<string> GetPreferencesPath()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(folder, "WeatherApp\\preferences.json");
        }

        public static async Task Save<T>(string key, T value)
        {
            var path = await GetPreferencesPath();
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(path);
            }
            var data = File.Exists(path) ? JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(path)) : new Dictionary<string, string>();
            data[key] = JsonSerializer.Serialize(value);
            await File.WriteAllTextAsync(path, JsonSerializer.Serialize(data));
        }

        public static async Task<T> Load<T>(string key, T defaultValue = default)
        {
            var path = await GetPreferencesPath();
            if (!File.Exists(path)) return defaultValue;

            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(path));
            return data != null && data.ContainsKey(key) ? JsonSerializer.Deserialize<T>(data[key]) : defaultValue;
        }
    }

}
