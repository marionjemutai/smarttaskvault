using SmartTaskVault.Models;
using System.Text.Json;

namespace SmartTaskVault.Services
{
    public class TaskManager
    {
        private List<TaskItem> tasks = new List<TaskItem>();

        public void AddTask(string title)
        {
            var newTask = new TaskItem
            {
                Id = tasks.Count + 1,
                Title = title,
                IsCompleted = false,
            };

            tasks.Add(newTask);
        }

        public List<TaskItem> GetAllTasks()
        {
            return tasks;
        }

        public List<TaskItem> GetPendingTasks()
        {
            return tasks.Where(t => !t.IsCompleted).ToList();
        }

        public async Task SaveTasksAsync()
        {
            var json = JsonSerializer.Serialize(tasks);
            await File.WriteAllTextAsync("tasks.json", json);
        }
        public async Task LoadTasksAsync()
        {
            if (File.Exists("tasks.json"))
            {
                string json = await File.ReadAllTextAsync("tasks.json");
                tasks = JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
            }
        }
    }
}