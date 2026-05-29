using MySql.Data.MySqlClient;
using SmartTaskVaultAPI.Config;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SmartTaskVaultAPI.Services
{
    public class AuthService
    {
        private readonly string connStr = "Server=localhost;Database=smartTaskVault;Uid=root;Pwd=3814;";

        public void Register(string u, string p)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(p);

            using var conn = new MySqlConnection(connStr);
            conn.Open();

            var cmd = new MySqlCommand(
                "INSERT INTO Users (Username, Password) VALUES (@u, @p)", conn);

            cmd.Parameters.AddWithValue("@u", u);
            cmd.Parameters.AddWithValue("@p", hashedPassword);

            cmd.ExecuteNonQuery();
        }

        public string? Login(string u, string p)   
        {
            using var conn = new MySqlConnection(connStr);
            conn.Open();

            var cmd = new MySqlCommand(
                "SELECT Password FROM Users WHERE Username=@u", conn);

            cmd.Parameters.AddWithValue("@u", u);

            using var reader = cmd.ExecuteReader();   

            if (!reader.Read()) return null;          

            string storedHash = reader.GetString("Password");
            reader.Close();

            // Verify the plain password against the stored hash
            if (!BCrypt.Net.BCrypt.Verify(p, storedHash)) return null;  

            // Build JWT token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, u)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: JwtConfig.Issuer,
                audience: JwtConfig.Issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),   
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}