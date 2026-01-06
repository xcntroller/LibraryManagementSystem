using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Validators
{
    // Validates that the ISBN is in a valid ISBN-10 or ISBN-13 format
    public class ValidIsbnAttribute : ValidationAttribute
    {
        public ValidIsbnAttribute()
        {
            ErrorMessage = "Invalid ISBN format. Please provide a valid ISBN-10 or ISBN-13.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("ISBN is required.");

            if (value is not string isbn)
                return new ValidationResult("ISBN must be a string.");

            if (IsbnValidator.IsValidISBN(isbn))
                return ValidationResult.Success;

            return new ValidationResult(ErrorMessage);
        }
    }
}