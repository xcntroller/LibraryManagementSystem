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
        public int BookId { get; set; }
        public string MemberName { get; set; } = null!;
    }

    public class LoanHistoryDto
    {
        public int Id { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; } = null!;
        public string MemberName { get; set; } = null!;

    }
}
