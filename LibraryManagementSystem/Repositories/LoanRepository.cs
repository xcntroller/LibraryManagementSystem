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
            var query = _context.Loans.AsQueryable();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(l => l.Book.Name.Contains(filter) || l.MemberName.Contains(filter));
            }
            return await query.ToListAsync();
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
            return await _context.Loans.Where(l => l.BookId == bookId && l.ReturnedAt == null).ToListAsync();
        }

        public async Task<List<Loan>> GetLoanHistoryByMemberAsync(string memberName)
        {
            return await _context.Loans
                .Where(l => l.MemberName == memberName)
                .OrderByDescending(l => l.LoanDate)
                .ToListAsync();
        }

        public async Task<List<Loan>> GetLoanHistoryByBookIdAsync(int bookId)
        {
            return await _context.Loans
                .Where(l => l.BookId == bookId)
                .OrderByDescending(l => l.LoanDate)
                .ToListAsync();
        }

        public async Task<List<Loan>> GetActiveLoansAsync()
        {
            return await _context.Loans.Where(l => l.ReturnedAt == null).ToListAsync();
        }

        public async Task<List<Loan>> GetOverdueLoansAsync()
        {
            return await _context.Loans.Where(l => l.ReturnedAt == null && l.ReturnDate < DateTime.Now).ToListAsync();
        }

        public async Task ReturnLoanAsync(int loanId)
        {
            var loan = await _context.Loans.FindAsync(loanId);
            if (loan != null && loan.ReturnedAt == null)
            {
                loan.ReturnedAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
    }
}
