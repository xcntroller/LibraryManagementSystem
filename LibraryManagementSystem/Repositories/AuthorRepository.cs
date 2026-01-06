using LibraryManagementSystem.Data;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryDbContext _context;
        public AuthorRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Author>> GetAllAsync(string? filter = null)
        {
            var query = _context.Authors.AsQueryable();
            if(!string.IsNullOrEmpty(filter))
            {
                query = query.Where(a => a.FirstName.Contains(filter) ||  a.LastName.Contains(filter));
            }
            return await query.ToListAsync();
        }

        public async Task<(List<Author> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? filter = null)
        {
            var query = _context.Authors.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(a => a.FirstName.Contains(filter) || a.LastName.Contains(filter));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Author?> GetByIdAsync(int id)
        {
            return await _context.Authors.Include(a => a.Books).SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> HasBooksAsync(int id)
        {
            return await _context.Books.AnyAsync(b => b.AuthorId == id);
        }

        public async Task<List<Book>> GetAuthorsBooksAsync(int authorId)
        {
            return await _context.Authors.Where(a => a.Id == authorId).SelectMany(a => a.Books).ToListAsync();
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.Authors.AnyAsync(a => a.Id == id);
        }

        public async Task AddAsync(Author author)
        {
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Author author)
        {
            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Author author)
        {
            if (await HasBooksAsync(author.Id))
                return false;

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalAuthorsCountAsync()
        {
            return await _context.Authors.CountAsync();
        }
    }
}
