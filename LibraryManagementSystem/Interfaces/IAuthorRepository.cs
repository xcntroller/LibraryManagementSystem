using LibraryManagementSystem.Entities;

namespace LibraryManagementSystem.Interfaces
{
    public interface IAuthorRepository
    {
        Task<List<Author>> GetAllAsync(string? filter = null);
        Task<Author?> GetByIdAsync(int id);
        Task<List<Book>> GetAuthorsBooksAsync(int authorId);
        Task<bool> ExistsByIdAsync(int id);
        Task<bool> HasBooksAsync(int id);
        Task AddAsync(Author author);
        Task UpdateAsync(Author author);
        Task DeleteAsync(Author author);
    }
}