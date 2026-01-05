namespace LibraryManagementSystem.DTOs
{

    public class CreateBookDto // author creating a new book
    {
        public string Name { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int PublicationYear { get; set; }
        public int PcsTotal { get; set; }
        public int AuthorId { get; set; }
    }

    public class BookDetailDto // all details of a book
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int PublicationYear { get; set; }
        public int PcsTotal { get; set; }
        public int PcsInStock { get; set; }
        public int AuthorId { get; set; }

    }

    public class ListBookDto // listing books with basic info
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int PublicationYear { get; set; }
        public int PcsInStock { get; set; }
        public int AuthorId { get; set; }
    }

    public class UpdateBookDto // updating book info
    {
        public string Name { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int PublicationYear { get; set; }
        public int PcsTotal { get; set; }
        public int AuthorId { get; set; }
    }
}
