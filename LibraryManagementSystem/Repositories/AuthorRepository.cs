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

        public async Task<Author?> GetByIdAsync(int id)
        {
            return await _context.Authors.Include(a => a.Books).SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> HasBooksAsync(int id)
        {
            return await _context.Books.AnyAsync(b => b.AuthorId == id);
        }

        public async Task<Author?> GetByNameAsync(string firstName, string lastName)
        {
            return await _context.Authors.Include(a => a.Books).SingleOrDefaultAsync(a => a.FirstName == firstName && a.LastName == lastName);
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

        public async Task DeleteAsync(Author author)
        {
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }
    }
}
