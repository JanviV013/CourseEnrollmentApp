using Microsoft.AspNetCore.Mvc;
using CourseEnrollmentApp.Models;
using System.Text.Json;

namespace CourseEnrollmentApp.Controllers
{
    // Controller dedicated to fetching external user data
    public class UserController : Controller
    {
        // Private field for HttpClient factory
        private readonly IHttpClientFactory _httpClientFactory;

        // Inject IHttpClientFactory through constructor
        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Asynchronous Action Method to fetch users from external REST API
        public async Task<IActionResult> Index()
        {
            List<UserViewModel> users = new List<UserViewModel>();

            // Create client instance
            HttpClient client = _httpClientFactory.CreateClient();

            // External API URL
            string apiUrl = "https://jsonplaceholder.typicode.com/users";

            // Asynchronously send GET request
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            // Check if HTTP request succeeded
            if (response.IsSuccessStatusCode)
            {
                // Read response content asynchronously
                string jsonData = await response.Content.ReadAsStringAsync();

                // Deserialize JSON into List<UserViewModel>
                users = JsonSerializer.Deserialize<List<UserViewModel>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            // Pass list to the View
            return View(users);
        }
    }
}