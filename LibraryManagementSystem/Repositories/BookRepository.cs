using LibraryManagementSystem.Data;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }
            
        public async Task<List<Book>> GetAllAsync(string? filter = null)
        {
            var query = _context.Books.AsQueryable();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(b => b.Name.Contains(filter) || b.ISBN.Contains(filter));
            }
            return await query.ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book?> GetByISBNAsync(string ISBN)
        {
            return await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.ISBN == ISBN);
        }

        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Book book)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasActiveLoansAsync(int bookId)
        {
            return await _context.Loans.AnyAsync(l => l.BookId == bookId && l.ReturnedAt == null);
        }

        public async Task<bool> IsAvailableAsync(int bookId)
        {
            return await _context.Books.AnyAsync(b => b.Id == bookId && b.PcsInStock > 0);
        }

        public async Task DecrementStockAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book != null && book.PcsInStock > 0)
            {
                book.PcsInStock--;
                await _context.SaveChangesAsync();
            }
        }

        public async Task IncrementStockAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book != null)
            {
                book.PcsInStock++;
                await _context.SaveChangesAsync();
            }
        }
    }
}
