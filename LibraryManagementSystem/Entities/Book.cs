using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [RegularExpression(@"^(?:\d{10}|\d{13}|97[89]\d{10})$", ErrorMessage = "ISBN must be 10 or 13 digits (ISBN-10 or ISBN-13 format)")]
        public string ISBN { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int PublicationYear { get; set; }
        public int PcsTotal { get; set; }
        public int PcsInStock { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;

        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}