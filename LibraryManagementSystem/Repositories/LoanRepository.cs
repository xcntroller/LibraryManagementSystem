using LibraryManagementSystem.Data;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly LibraryDbContext _context;

        public LoanRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Loan>> GetAllAsync(string? filter = null)
        {
            var query = _context.Loans.Include(l => l.Book).AsQueryable();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(l => l.Book.Name.Contains(filter) || l.MemberName.Contains(filter));
            }
            return await query.ToListAsync();
        }

        public async Task<(List<Loan> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? filter = null)
        {
            var query = _context.Loans.AsNoTracking().Include(l => l.Book).AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(l => l.Book.Name.Contains(filter) || l.MemberName.Contains(filter));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(l => l.LoanDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Loan?> GetByIdAsync(int id)
        {
            return await _context.Loans.SingleOrDefaultAsync(l => l.Id == id);
        }

        public async Task AddAsync(Loan loan)
        {
            await _context.Loans.AddAsync(loan);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Loan>> GetActiveLoansByBookIdAsync(int bookId)
        {
            return await _context.Loans
                .AsNoTracking()
                .Where(l => l.BookId == bookId && l.ReturnedAt == null)
                .ToListAsync();
        }

        public async Task<List<Loan>> GetLoanHistoryByMemberAsync(string memberName)
        {
            return await _context.Loans
                .AsNoTracking()
                .Where(l => l.MemberName == memberName)
                .OrderByDescending(l => l.LoanDate)
                .ToListAsync();
        }

        public async Task<List<Loan>> GetLoanHistoryByBookIdAsync(int bookId)
        {
            return await _context.Loans
                .AsNoTracking()
                .Where(l => l.BookId == bookId)
                .OrderByDescending(l => l.LoanDate)
                .ToListAsync();
        }

        public async Task<List<Loan>> GetActiveLoansAsync()
        {
            return await _context.Loans
                .AsNoTracking()
                .Where(l => l.ReturnedAt == null)
                .ToListAsync();
        }

        public async Task<List<Loan>> GetOverdueLoansAsync()
        {
            var now = DateTime.Now;
            return await _context.Loans
                .AsNoTracking()
                .Where(l => l.ReturnedAt == null && l.ReturnDate < now)
                .ToListAsync();
        }

        public async Task ReturnLoanAsync(int loanId)
        {
            var now = DateTime.Now;
            await _context.Loans
                .Where(l => l.Id == loanId && l.ReturnedAt == null)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(l => l.ReturnedAt, now));
        }

        public async Task<List<(int BookId, string BookName, string ISBN, int BorrowCount, int AuthorId, string AuthorName)>> GetMostBorrowedBooksAsync(int topCount = 10)
        {
            return await _context.Loans
                .AsNoTracking()
                .Include(l => l.Book)
                .ThenInclude(b => b.Author)
                .GroupBy(l => new { l.BookId, l.Book.Name, l.Book.ISBN, l.Book.AuthorId, l.Book.Author.FirstName, l.Book.Author.LastName })
                .Select(g => new
                {
                    g.Key.BookId,
                    g.Key.Name,
                    g.Key.ISBN,
                    BorrowCount = g.Count(),
                    g.Key.AuthorId,
                    AuthorName = g.Key.FirstName + " " + g.Key.LastName
                })
                .OrderByDescending(x => x.BorrowCount)
                .Take(topCount)
                .Select(x => ValueTuple.Create(x.BookId, x.Name, x.ISBN, x.BorrowCount, x.AuthorId, x.AuthorName))
                .ToListAsync();
        }

        public async Task<double> GetAverageLoanDurationAsync()
        {
            var completedLoans = await _context.Loans
                .AsNoTracking()
                .Where(l => l.ReturnedAt != null)
                .Select(l => new { l.LoanDate, l.ReturnedAt })
                .ToListAsync();

            if (completedLoans.Count == 0)
                return 0;

            var totalDays = completedLoans.Sum(l => (l.ReturnedAt!.Value - l.LoanDate).TotalDays);
            return totalDays / completedLoans.Count;
        }

        public async Task<int> GetTotalLoansCountAsync()
        {
            return await _context.Loans.CountAsync();
        }

        public async Task<int> GetCompletedLoansCountAsync()
        {
            return await _context.Loans.CountAsync(l => l.ReturnedAt != null);
        }

        public async Task<int> GetUniqueBorrowersCountAsync()
        {
            return await _context.Loans
                .Select(l => l.MemberName)
                .Distinct()
                .CountAsync();
        }
    }
}
