using LibraryManagementSystem.Entities;

namespace LibraryManagementSystem.Interfaces
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync(string? filter = null);
        Task<(List<Book> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? filter = null);
        Task<Book?> GetByIdAsync(int id);
        Task<Book?> GetByISBNAsync(string ISBN);
        Task<List<Book>> GetByAuthorIdAsync(int authorId);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(Book book);
        Task<bool> HasActiveLoansAsync(int bookId);
        Task DecrementStockAsync(int bookId);
        Task IncrementStockAsync(int bookId);

        //statistics
        Task<int> GetTotalBooksCountAsync();
        Task<int> GetAvailableBooksCountAsync();
    }
}
