
using System.Text.Json;

namespace SmartTaskVaultAPI.Config
{
    public static class DbConfig
    {
        public static string GetConnectionString()
        {
            string json = File.ReadAllText("appsettings.json");

            using JsonDocument doc = JsonDocument.Parse(json);

            return doc.RootElement
                .GetProperty("Database")
                .GetProperty("ConnectionString")
                .GetString()!;
        }
    }
}

