namespace CourseEnrollmentApp.Models
{
    // Class representing user data received from JSON API
    public class UserViewModel
    {
        // Property for User ID
        public int Id { get; set; }

        // Property for User Name
        public string Name { get; set; }

        // Property for User Email
        public string Email { get; set; }

        // Nested property representing the Company JSON object
        public CompanyInfo Company { get; set; }
    }

    // Class to map nested company information from JSON
    public class CompanyInfo
    {
        // Property for Company Name
        public string Name { get; set; }
    }
}