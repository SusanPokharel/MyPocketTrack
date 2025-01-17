using System.Text.Json;

namespace MyPocketTrack.Components.Storage
{
    public class FileHandler<T>
    {
        private static readonly string PreferencesFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyPocketTrack");

        public static string GetFilePath(string fileName)
        {
            if (!Directory.Exists(PreferencesFolder))
            {
                Directory.CreateDirectory(PreferencesFolder);
            }

            return Path.Combine(PreferencesFolder, fileName);
        }

        public static async Task<List<T>> ReadFromFile(string fileName)
        {
            string filePath = GetFilePath(fileName);

            if (!File.Exists(filePath))
            {
                return new List<T>();
            }

            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }

        public static async Task SaveToFile(string fileName, List<T> data)
        {
            string filePath = GetFilePath(fileName);
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, json);
        }
    }
}

