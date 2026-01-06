using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DTOs
{
    public class CreateAuthorDto
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "First name must be between 1 and 100 characters")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Last name must be between 1 and 100 characters")]
        public string LastName { get; set; } = null!;

        [StringLength(2000, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 2000 characters")]
        public string? Description { get; set; }

        [Range(1000, 2100, ErrorMessage = "Birth year must be between 1000 and 2100")]
        public int BirthYear { get; set; }
    }

    public class AuthorDetailDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Description { get; set; }

        [Range(1000, 2100)]
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
        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "First name must be between 1 and 100 characters")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Last name must be between 1 and 100 characters")]
        public string LastName { get; set; } = null!;

        [StringLength(2000, ErrorMessage = "Description must not exceed 2000 characters")]
        public string? Description { get; set; }

        [Range(1000, 2100, ErrorMessage = "Birth year must be between 1000 and 2100")]
        public int BirthYear { get; set; }
    }
}