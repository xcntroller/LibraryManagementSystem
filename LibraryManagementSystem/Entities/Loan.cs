namespace LibraryManagementSystem.Entities
{
    public class Loan
    {
        public int Id { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;
        public string MemberName { get; set; } = null!;
    }
}
