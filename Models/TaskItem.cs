namespace SmartTaskVaultAPI.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Status { get; set; }
        public DateTime CreatedAt { get; set; } 
        public bool IsCompleted { get; set; }
    }
}