namespace CourseEnrollmentApp.Models
{
    // Define the Course model class to represent course details
    public class Course
    {
        // Property to uniquely identify a course
        public int CourseId { get; set; }

        // Property to store the name of the course
        public string CourseName { get; set; }

        // Property to store the duration of the course (e.g., "3 Months")
        public string Duration { get; set; }

        // Property to store the base fee of the course
        public decimal Fees { get; set; }

        // Property to store the discount percentage applied to the course
        public decimal DiscountPercentage { get; set; }
    }
}