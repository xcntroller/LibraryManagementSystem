namespace LibraryManagementSystem.DTOs
{
    public class CreateLoanDto
    {
        public int BookId { get; set; }
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
