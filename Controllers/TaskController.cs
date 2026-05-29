// TaskController.cs
using SmartTaskVaultAPI.Services;
using System.Text.Json;

namespace SmartTaskVaultAPI.Controllers
{
    public class TaskController
    {
        private TaskService taskService = new TaskService();

        public string GetAll()
        {
            return JsonSerializer.Serialize(taskService.GetAll());
        }

        public string GetPending()
        {
            return JsonSerializer.Serialize(taskService.GetPending());
        }

        public string Add(System.Net.HttpListenerRequest request)
        {
            using var reader = new StreamReader(request.InputStream);
            string body = reader.ReadToEnd();

            if (string.IsNullOrEmpty(body))
                return "Empty request body";

            // Fix: parse JSON and extract the "title" field
            var json = JsonSerializer.Deserialize<JsonElement>(body);

            if (!json.TryGetProperty("title", out JsonElement titleElement))
                return "Missing 'title' field";

            string? title = titleElement.GetString();

            if (string.IsNullOrEmpty(title))
                return "Title cannot be empty";

            taskService.AddTask(title);
            return "Task added";
        }
    }
}