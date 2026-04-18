using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Selenium_CSharp_Framework.utilities
{
    public static class JsonReader
    {
        public static List<T> ReadList<T>(string relativePath)
        {
            string json = ReadFile(relativePath);
            return JsonConvert.DeserializeObject<List<T>>(json)
                   ?? throw new InvalidDataException($"Failed to deserialize: {relativePath}");
        }

        public static List<T> ReadSection<T>(string relativePath, string section)
        {
            string json = ReadFile(relativePath);
            JObject jsonObject = JObject.Parse(json);

            JToken sectionToken = jsonObject[section]
                ?? throw new KeyNotFoundException(
                    $"Section '{section}' not found in: {relativePath}");

            return sectionToken.ToObject<List<T>>()
                   ?? throw new InvalidDataException(
                    $"Failed to deserialize section '{section}' in: {relativePath}");
        }

        private static string ReadFile(string relativePath)
        {
            string normalizedPath = relativePath
                .Replace("/", Path.DirectorySeparatorChar.ToString())
                .Replace("\\", Path.DirectorySeparatorChar.ToString());

            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, normalizedPath);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Test data file not found: {fullPath}");

            return File.ReadAllText(fullPath);
        }
    }
}