namespace LibraryManagementSystem.Entities
{
    public class Author
    {
        public int Id { get; set; }

        public string firstName { get; set; } = null!;
        public string lastName { get; set; } = null!;
        public string description { get; set; } = null!;

        public int birthYear { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
