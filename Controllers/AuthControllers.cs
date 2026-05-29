// AuthController.cs
using SmartTaskVaultAPI.Services;
using System.Net;
using System.Text.Json;

namespace SmartTaskVaultAPI.Controllers
{
    public class AuthController
    {
        private AuthService Service = new AuthService();

        public string Register(HttpListenerRequest request)
        {
            using var reader = new StreamReader(request.InputStream);
            string body = reader.ReadToEnd();

            if (string.IsNullOrEmpty(body))
                return "Empty request body";

            // Fix: parse JSON instead of splitting by comma
            var json = JsonSerializer.Deserialize<JsonElement>(body);

            if (!json.TryGetProperty("username", out JsonElement userEl) ||
                !json.TryGetProperty("password", out JsonElement passEl))
                return "Missing 'username' or 'password' field";

            string? username = userEl.GetString();
            string? password = passEl.GetString();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return "Username and password cannot be empty";

            Service.Register(username, password);
            return "User registered";
        }

        public string Login(HttpListenerRequest request)
        {
            using var reader = new StreamReader(request.InputStream);
            string body = reader.ReadToEnd();

            if (string.IsNullOrEmpty(body))
                return "Empty request body";

            // Fix: parse JSON instead of splitting by comma
            var json = JsonSerializer.Deserialize<JsonElement>(body);

            if (!json.TryGetProperty("username", out JsonElement userEl) ||
                !json.TryGetProperty("password", out JsonElement passEl))
                return "Missing 'username' or 'password' field";

            string? username = userEl.GetString();
            string? password = passEl.GetString();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return "Username and password cannot be empty";

            string? token = Service.Login(username, password);
            if (token == null) return "Invalid credentials";
            return token;
        }
    }
}