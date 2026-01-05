using LibraryManagementSystem.Entities;

namespace LibraryManagementSystem.Interfaces
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync(string? filter = null);
        Task<Book?> GetByIdAsync(int id);
        Task<Book?> GetByISBNAsync(string ISBN);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(Book book);
        Task<bool> HasActiveLoansAsync(int bookId);
        Task<bool> IsAvailableAsync(int bookId);
        Task DecrementStockAsync(int bookId);
        Task IncrementStockAsync(int bookId);
    }
}
