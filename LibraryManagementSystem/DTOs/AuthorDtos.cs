using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DTOs
{
    public class CreateAuthorDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int BirthYear { get; set; }
    }

    public class AuthorDetailDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Description { get; set; }

        [Range(1000, 2030)]
        public int BirthYear { get; set; }
        public ICollection<ListBookDto> Books { get; set; } = new List<ListBookDto>();
    }

    public class ListAuthorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int BirthYear { get; set; }
    }

    public class UpdateAuthorDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Description { get; set; }

        [Range(1000, 2030)]
        public int BirthYear { get; set; }
    }
}
