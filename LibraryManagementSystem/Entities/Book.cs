namespace LibraryManagementSystem.Entities
{
    public class Book
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public int PublicationYear { get; set; }

        public int pcsTotal { get; set; }
        public int pcsInStock { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;

        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}
