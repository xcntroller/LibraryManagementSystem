using LibraryManagementSystem.Entities;

namespace LibraryManagementSystem.Interfaces
{
    public interface IAuthorRepository
    {
        Task<List<Author>> GetAllAsync(string? filter = null);
        Task<Author?> GetByIdAsync(int id);
        Task<Author?> GetByNameAsync(string firstName, string lastName);
        Task<List<Book>> GetAuthorsBooksAsync(int authorId);
        Task<bool> ExistsByIdAsync(int id);
        Task AddAsync(Author author);
        Task UpdateAsync(Author author);
        Task DeleteAsync(Author author);
    }
}