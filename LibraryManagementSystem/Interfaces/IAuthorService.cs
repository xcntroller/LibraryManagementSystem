using LibraryManagementSystem.DTOs;

namespace LibraryManagementSystem.Interfaces
{
    public interface IAuthorService
    {
        Task<List<ListAuthorDto>> GetAllAuthorsAsync(string? filter);
        Task<PagedResultDto<ListAuthorDto>> GetPagedAuthorsAsync(int pageNumber, int pageSize, string? filter);
        Task<AuthorDetailDto?> GetAuthorByIdAsync(int id);
        Task<List<ListBookDto>> GetAuthorsBooksAsync(int authorId);
        Task<bool> AuthorExistsByIdAsync(int id);
        Task<AuthorDetailDto?> CreateAuthorAsync(CreateAuthorDto dto);
        Task<bool> UpdateAuthorAsync(int id, UpdateAuthorDto dto);
        Task<bool> DeleteAuthorAsync(int id);
    }
}