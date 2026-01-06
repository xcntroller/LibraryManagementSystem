using LibraryManagementSystem.DTOs;

namespace LibraryManagementSystem.Interfaces
{
    public interface IBookService
    {
        Task<List<ListBookDto>> GetAllBooksAsync(string? filter);
        Task<PagedResultDto<ListBookDto>> GetPagedBooksAsync(int pageNumber, int pageSize, string? filter);
        Task<BookDetailDto?> GetBookByIdAsync(int id);
        Task<BookDetailDto?> GetBookByISBNAsync(string isbn);
        Task<List<ListBookDto>> GetBooksByAuthorIdAsync(int authorId);
        Task<BookDetailDto?> CreateBookAsync(CreateBookDto dto);
        Task<bool> UpdateBookAsync(int id, UpdateBookDto dto);
        Task<bool> DeleteBookAsync(int id);
    }
}