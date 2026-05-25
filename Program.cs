using SmartTaskVault.Services;

TaskManager manager = new TaskManager();
await manager.LoadTasksAsync();

while (true)
{
    Console.WriteLine("\n===Smart Task Vault===");
    Console.WriteLine("1. Add Task");
    Console.WriteLine("2. View All Tasks");
    Console.WriteLine("3. View Pending Tasks");
    Console.WriteLine("4. Save Tasks");
    Console.WriteLine("5. Exit");

    Console.Write("Choose an option: ");
    string? choiceInput = Console.ReadLine();
    int choice = Convert.ToInt32(choiceInput ?? "0");

    switch (choice)
    {
        case 1:
            Console.Write("Enter task title: ");
            string title = Console.ReadLine() ?? string.Empty;
            manager.AddTask(title);
            Console.WriteLine("Task added!");
            break;
        case 2:
            var tasks = manager.GetAllTasks();
            foreach (var task in tasks)
            {
                Console.WriteLine($"{task.Id}. {task.Title} - {(task.IsCompleted ? "Completed" : "Pending")}");
            }
            
            break;
        case 3:
            var pendingTasks = manager.GetPendingTasks();
            foreach (var task in pendingTasks)
            {
                Console.WriteLine($"{task.Id}. {task.Title}");
            }
            break;
        case 4:
            await manager.SaveTasksAsync();
            Console.WriteLine("Tasks saved!");
            break;
        case 5:
            return;
        default:
            Console.WriteLine("Invalid option, try again.");
            break;
    }
}
