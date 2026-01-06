using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DTOs
{
    public class CreateLoanDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Book ID must be a positive number")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Member name is required")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Member name must be between 2 and 200 characters")]
        [RegularExpression(@"^[a-zA-ZáčďéěíňóřšťúůýžÁČĎÉĚÍŇÓŘŠŤÚŮÝŽ\s'-]+$", ErrorMessage = "Member name can only contain letters, spaces, hyphens, and apostrophes")]
        public string MemberName { get; set; } = null!;
    }

    public class ListLoansDto
    {
        public int Id { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public int BookId { get; set; }
        public string MemberName { get; set; } = null!;
        public bool IsOverdue { get; set; }
    }

    public class LoanDetailDto
    {
        public int Id { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public int BookId { get; set; }
        public string MemberName { get; set; } = null!;
        public bool IsOverdue { get; set; }
        public string? WarningMessage { get; set; }
    }
}