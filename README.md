# SmartTaskVault API

A RESTful API built from scratch in **C# (.NET 10)** — no frameworks, raw HTTP server using `HttpListener`. Handles user authentication with JWT tokens and task management backed by a MySQL database.

---

## Tech Stack

| Technology | Purpose |
|---|---|
| C# / .NET 10 | Core language and runtime |
| MySQL | Relational database |
| JWT (JSON Web Tokens) | Stateless user authentication |
| BCrypt | Secure password hashing |
| HttpListener | Raw HTTP server (no ASP.NET) |

---

## Project Structure

```
SmartTaskVault/
├── Core/
│   └── Router.cs               # HTTP listener, request routing
├── Controllers/
│   ├── AuthController.cs       # Register and login endpoints
│   └── TaskController.cs       # Task CRUD endpoints
├── Services/
│   ├── AuthService.cs          # Auth logic, JWT generation
│   └── TaskService.cs          # Task database operations
├── Models/
│   ├── Users.cs                # User model
│   └── TaskItem.cs             # Task model
└── Config/
    └── JwtConfig.cs            # JWT secret key and issuer
```

---

## API Endpoints

| Method | Endpoint | Body | Description |
|---|---|---|---|
| `POST` | `/register` | `{ "username", "password" }` | Register a new user |
| `POST` | `/login` | `{ "username", "password" }` | Login and receive JWT token |
| `GET` | `/tasks` | — | Get all tasks |
| `POST` | `/tasks` | `{ "title" }` | Create a new task |

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- MySQL Server running locally
- MySQL database and tables set up (see below)

### Database Setup

Open MySQL and run:

```sql
CREATE DATABASE smartTaskVault;

USE smartTaskVault;

CREATE TABLE Users (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(100) UNIQUE,
    Password VARCHAR(255)
);

CREATE TABLE Tasks (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(255),
    Status VARCHAR(50),
    CreatedAt DATETIME
);
```

### Configuration

In `Services/AuthService.cs` and `Services/TaskService.cs`, update the connection string with your MySQL credentials:

```csharp
string connStr = "Server=localhost;Database=smartTaskVault;Uid=root;Pwd=YOUR_PASSWORD;";
```

### Install Dependencies

```bash
dotnet add package MySql.Data
dotnet add package BCrypt.Net-Next
dotnet add package Microsoft.IdentityModel.Tokens
dotnet add package System.IdentityModel.Tokens.Jwt
```

### Run the Server

```bash
dotnet run
```

Server starts at: `http://localhost:5000`

---

## Testing with Thunder Client (or Postman)

> Use `http://` — NOT `https://`

### Register
```
POST http://localhost:5000/register
Body (JSON):
{
  "username": "testuser",
  "password": "test123"
}
```

### Login
```
POST http://localhost:5000/login
Body (JSON):
{
  "username": "testuser",
  "password": "test123"
}
```
Returns a JWT token string.

### Add a Task
```
POST http://localhost:5000/tasks
Body (JSON):
{
  "title": "My first task"
}
```

### Get All Tasks
```
GET http://localhost:5000/tasks
```

---

## Security

- Passwords are hashed with **BCrypt** before being stored — plain text passwords are never saved
- Authentication uses **JWT tokens** signed with a secret key, expiring after 1 hour
- SQL injection is prevented via **parameterised queries** throughout

---

## Key Design Decisions

**No framework** — Built on raw `HttpListener` to demonstrate deep understanding of how HTTP servers handle requests, routing, and responses at a low level.

**Layered architecture** — Router handles routing only. Controllers handle request/response parsing only. Services handle all business logic and database access. Models define data shapes.

**BCrypt over plain hashing** — BCrypt is intentionally slow, making brute-force attacks against stored passwords computationally expensive even if the database is compromised.

---

## Author

Built by [Your Name]
