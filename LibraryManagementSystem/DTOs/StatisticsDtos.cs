namespace LibraryManagementSystem.DTOs
{
    public class MostBorrowedBookDto
    {
        public int BookId { get; set; }
        public string BookName { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public int BorrowCount { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = null!;
    }

    public class LoanStatisticsDto
    {
        public double AverageLoanDurationDays { get; set; }
        public int TotalLoans { get; set; }
        public int ActiveLoans { get; set; }
        public int CompletedLoans { get; set; }
        public int OverdueLoans { get; set; }
    }

    public class LibraryStatisticsDto
    {
        public int TotalBooks { get; set; }
        public int TotalAuthors { get; set; }
        public int TotalAvailableBooks { get; set; }
        public int UniqueBorrowers { get; set; }
        public List<MostBorrowedBookDto> MostBorrowedBooks { get; set; } = new();
        public LoanStatisticsDto LoanStatistics { get; set; } = null!;
    }
}