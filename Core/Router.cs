using System.Net;
using SmartTaskVaultAPI.Controllers;
using System.Text;

namespace SmartTaskVaultAPI.Core
{
    public class Router
    {
        public static void Start()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5000/");
            listener.Start();
            Console.WriteLine("Server started on http://localhost:5000/");
            Console.WriteLine("Use http:// (NOT https://) in Thunder Client or any HTTP client");
            Console.WriteLine("Endpoints:");
            Console.WriteLine("  POST http://localhost:5000/register - Body: username,password");
            Console.WriteLine("  POST http://localhost:5000/login - Body: username,password");
            Console.WriteLine("  GET  http://localhost:5000/tasks");
            Console.WriteLine("  POST http://localhost:5000/tasks - Body: task title");

            while (true)
            {
                var context = listener.GetContext();
                var request = context.Request;
                var response = context.Response;
                string result = "";

                var taskController = new TaskController();
                var authController = new AuthController();

                string path = request.Url?.AbsolutePath.ToLower() ?? "/";
                string method = request.HttpMethod;

                if (path == "/register" && method == "POST")
                {
                    result = authController.Register(request);
                }
                else if (path == "/login" && method == "POST")
                {
                    result = authController.Login(request);
                }
                else if (path == "/tasks" && method == "GET")
                {
                    result = taskController.GetAll();
                }
                else if (path == "/tasks" && method == "POST")
                {
                    result = taskController.Add(request);
                }
                else
                {
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    result = "Not Found";
                }

                response.ContentType = "application/json";
                response.ContentEncoding = Encoding.UTF8;
                byte[] data = Encoding.UTF8.GetBytes(result);
                response.OutputStream.Write(data, 0, data.Length);
                response.Close();
            }
        }
    }
}