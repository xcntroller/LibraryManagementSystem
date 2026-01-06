using LibraryManagementSystem.Validators;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DTOs
{

    public class CreateBookDto // author creating a new book
    {
        [Required(ErrorMessage = "Book name is required")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Book name must be between 1 and 200 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "ISBN is required")]
        [ValidIsbn]
        public string ISBN { get; set; } = null!;

        [StringLength(2000, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 2000 characters")]
        public string? Description { get; set; }

        [Range(1000, 2100, ErrorMessage = "Publication year must be between 1000 and 2100")]
        public int PublicationYear { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Total pieces must be at least 1")]
        public int PcsTotal { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Author ID must be a positive number")]
        public int AuthorId { get; set; }
    }

    public class BookDetailDto // all details of a book
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public string? Description { get; set; }
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
        [Required(ErrorMessage = "Book name is required")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Book name must be between 1 and 200 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "ISBN is required")]
        [ValidIsbn]
        public string ISBN { get; set; } = null!;

        [StringLength(2000, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 2000 characters")]
        public string? Description { get; set; }

        [Range(1000, 2100, ErrorMessage = "Publication year must be between 1000 and 2100")]
        public int PublicationYear { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Total pieces must be at least 1")]
        public int PcsTotal { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Author ID must be a positive number")]
        public int AuthorId { get; set; }
    }
}
