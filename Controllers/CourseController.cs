using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using CourseEnrollmentApp.Models;
using System.Text.Json;

namespace CourseEnrollmentApp.Controllers
{
    // Single controller handling all operations for Task A
    public class CourseController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        // Inject Memory Cache and HttpClient Factory via constructor
        public CourseController(IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }

        // -------------------------------------------------------------
        // 1. COURSE LIST & IMEMORYCACHE (Task A - Section 4)
        // URL: /Course or /
        // -------------------------------------------------------------
        public IActionResult Index()
        {
            string cacheKey = "CourseListCache";

            // Check if course list exists in memory cache
            if (!_memoryCache.TryGetValue(cacheKey, out List<Course> courseList))
            {
                // Instantiate sample data on cache miss
                courseList = new List<Course>
                {
                    new Course { CourseId = 101, CourseName = "ASP.NET Core MVC", Duration = "3 Months", Fees = 8000m, DiscountPercentage = 10m },
                    new Course { CourseId = 102, CourseName = "Full Stack Web Dev", Duration = "6 Months", Fees = 15000m, DiscountPercentage = 20m },
                    new Course { CourseId = 103, CourseName = "C# Programming Basics", Duration = "2 Months", Fees = 5000m, DiscountPercentage = 5m }
                };

                // Cache for 5 minutes as required
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetPriority(CacheItemPriority.Normal);

                _memoryCache.Set(cacheKey, courseList, cacheOptions);
                ViewBag.CacheStatus = "Data fetched fresh and saved in IMemoryCache!";
            }
            else
            {
                ViewBag.CacheStatus = "Data retrieved directly from IMemoryCache!";
            }

            ViewBag.FetchTime = DateTime.Now.ToString("HH:mm:ss");
            return View(courseList);
        }

        // -------------------------------------------------------------
        // 2. MODEL, EXTENSION METHOD & RESPONSE CACHING (Task A - Section 1 & 4)
        // URL: /Course/Details
        // -------------------------------------------------------------
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Details()
        {
            Course sampleCourse = new Course
            {
                CourseId = 101,
                CourseName = "ASP.NET Core Web Development",
                Duration = "6 Months",
                Fees = 10000m,
                DiscountPercentage = 15m
            };

            // Extension method call
            decimal finalFees = sampleCourse.CalculateFinalFees();
            ViewBag.FinalFees = finalFees;
            ViewBag.RenderTime = DateTime.Now.ToString("HH:mm:ss");

            return View(sampleCourse);
        }

        // -------------------------------------------------------------
        // 3. ASYNCHRONOUS WEB REQUEST (Task A - Section 2)
        // URL: /Course/Users
        // -------------------------------------------------------------
        public async Task<IActionResult> Users()
        {
            List<UserViewModel> users = new List<UserViewModel>();
            HttpClient client = _httpClientFactory.CreateClient();
            string apiUrl = "https://jsonplaceholder.typicode.com/users";

            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonData = await response.Content.ReadAsStringAsync();
                users = JsonSerializer.Deserialize<List<UserViewModel>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            return View(users);
        }

        // -------------------------------------------------------------
        // 4. STATE MANAGEMENT: ENROLLMENT FORM & COOKIE/SESSION/QUERY STRING/HIDDEN FIELD (Task A - Section 3)
        // URL: /Course/Enroll?courseId=101
        // -------------------------------------------------------------
        public IActionResult Enroll(int courseId)
        {
            // Query String
            ViewBag.CourseIdFromQueryString = courseId;

            // Cookie read
            ViewBag.SavedStudentName = Request.Cookies["StudentName"];

            // Session read
            ViewBag.SavedCourseFromSession = HttpContext.Session.GetString("SelectedCourse");

            var model = new EnrollmentViewModel
            {
                CourseId = courseId,
                StudentName = ViewBag.SavedStudentName
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult SubmitEnrollment(EnrollmentViewModel model)
        {
            // Save Cookie (Student Name)
            CookieOptions options = new CookieOptions { Expires = DateTime.Now.AddDays(7), HttpOnly = true };
            Response.Cookies.Append("StudentName", model.StudentName ?? "", options);

            // Save Session (Selected Course)
            HttpContext.Session.SetString("SelectedCourse", "ASP.NET Core MVC (ID: " + model.CourseId + ")");

            ViewBag.SubmittedCourseId = model.CourseId;
            ViewBag.SubmittedStudentName = model.StudentName;

            return View("Success");
        }
    }
}