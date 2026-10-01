namespace CourseEnrollmentApp.Models
{
    // Static class containing extension methods for the Course model
    public static class CourseExtensions
    {
        // Extension method to calculate final fees after applying the discount percentage
        // 'this Course course' binds this method directly to any Course object instance
        public static decimal CalculateFinalFees(this Course course)
        {
            // Safeguard against null course references
            if (course == null)
                return 0;

            // Calculate the discount amount: Fees * (DiscountPercentage / 100)
            decimal discountAmount = course.Fees * (course.DiscountPercentage / 100m);

            // Subtract discount amount from base fees to calculate final fees
            decimal finalFees = course.Fees - discountAmount;

            // Return the calculated final fees
            return finalFees;
        }
    }
}