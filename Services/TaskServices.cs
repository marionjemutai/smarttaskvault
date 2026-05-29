
using MySql.Data.MySqlClient;
using SmartTaskVaultAPI.Models;
using SmartTaskVaultAPI.Config;

namespace SmartTaskVaultAPI.Services
{
    public class TaskService
    {
        private readonly string connStr = DbConfig.GetConnectionString();

        public void AddTask(string title)
        {
            using var conn = new MySqlConnection(connStr);
            conn.Open();

            var cmd = new MySqlCommand(
                "INSERT INTO Tasks (Title, Status, CreatedAt) VALUES (@t,'Pending',NOW())",
                conn);

            cmd.Parameters.AddWithValue("@t", title);
            cmd.ExecuteNonQuery();
        }

        public List<TaskItem> GetAll()
        {
            var list = new List<TaskItem>();

            using var conn = new MySqlConnection(connStr);
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM Tasks", conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new TaskItem
                {
                    Id = reader.GetInt32("Id"),
                    Title = reader.GetString("Title"),
                    Status = reader.GetString("Status"),
                    CreatedAt = reader.GetDateTime("CreatedAt")
                });
            }

            return list;
        }

        public List<TaskItem> GetPending()
        {
            var list = new List<TaskItem>();

            using var conn = new MySqlConnection(connStr);
            conn.Open();

            var cmd = new MySqlCommand(
                "SELECT * FROM Tasks WHERE Status='Pending'",
                conn);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new TaskItem
                {
                    Id = reader.GetInt32("Id"),
                    Title = reader.GetString("Title"),
                    Status = reader.GetString("Status"),
                    CreatedAt = reader.GetDateTime("CreatedAt")
                });
            }

            return list;
        }
    }
}

