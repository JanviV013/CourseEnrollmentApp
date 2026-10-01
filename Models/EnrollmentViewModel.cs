namespace CourseEnrollmentApp.Models
{
    // Model used to bind form data during course enrollment
    public class EnrollmentViewModel
    {
        // Hidden field value: submitted silently via form
        public int CourseId { get; set; }

        // Student Name entered in the form
        public string StudentName { get; set; }
    }
}