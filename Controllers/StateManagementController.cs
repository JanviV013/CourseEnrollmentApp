using Microsoft.AspNetCore.Mvc;
using CourseEnrollmentApp.Models;

namespace CourseEnrollmentApp.Controllers
{
    public class StateManagementController : Controller
    {
        // 1. QUERY STRING & HIDDEN FIELD: Display Enrollment Form
        // Access via URL like: /StateManagement/Enroll?courseId=101
        public IActionResult Enroll(int courseId)
        {
            // Read courseId passed via Query String
            ViewBag.CourseIdFromQueryString = courseId;

            // Retrieve Student Name from Cookie if it exists
            string savedStudentName = Request.Cookies["StudentName"];
            ViewBag.SavedStudentName = savedStudentName;

            // Retrieve Selected Course from Session if it exists
            string savedCourse = HttpContext.Session.GetString("SelectedCourse");
            ViewBag.SavedCourseFromSession = savedCourse;

            // Initialize ViewModel passing CourseId to be rendered in a Hidden Field
            var model = new EnrollmentViewModel
            {
                CourseId = courseId,
                StudentName = savedStudentName
            };

            return View(model);
        }

        // 2. COOKIE & SESSION & HIDDEN FIELD: Process Form Post
        [HttpPost]
        public IActionResult SubmitEnrollment(EnrollmentViewModel model)
        {
            // COOKIE: Store Student Name in an HTTP Cookie expiring in 7 days
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
                HttpOnly = true
            };
            Response.Cookies.Append("StudentName", model.StudentName ?? "", options);

            // SESSION: Store selected course in Session memory
            HttpContext.Session.SetString("SelectedCourse", "ASP.NET Core MVC (ID: " + model.CourseId + ")");

            // Pass confirmation details via ViewBag
            ViewBag.SubmittedCourseId = model.CourseId; // Passed via Hidden Field
            ViewBag.SubmittedStudentName = model.StudentName;

            return View("Success");
        }
    }
}