using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Interfaces;

namespace LibraryManagementSystem.Services
{
    public class LoanService
    {
        private readonly ILoanRepository _loanRepo;
        private readonly IBookRepository _bookRepo;

        public LoanService(ILoanRepository loanRepo, IBookRepository bookRepo)
        {
            _loanRepo = loanRepo;
            _bookRepo = bookRepo;
        }

        public async Task<List<ListLoansDto>> GetAllLoansAsync(string? filter)
        {
            var loans = await _loanRepo.GetAllAsync(filter);
            return loans.Select(l => new ListLoansDto
            {
                Id = l.Id,
                LoanDate = l.LoanDate,
                ReturnDate = l.ReturnDate,
                ReturnedAt = l.ReturnedAt,
                BookId = l.BookId,
                MemberName = l.MemberName
            }).ToList();
        }

        public async Task<LoanDetailDto?> GetLoanByIdAsync(int id)
        {
            var loan = await _loanRepo.GetByIdAsync(id);
            if (loan == null) return null;

            return new LoanDetailDto
            {
                Id = loan.Id,
                LoanDate = loan.LoanDate,
                ReturnDate = loan.ReturnDate,
                ReturnedAt = loan.ReturnedAt,
                BookId = loan.BookId,
                MemberName = loan.MemberName
            };
        }

        public async Task<List<LoanHistoryDto>> GetLoanHistoryByMemberAsync(string memberName)
        {
            var loans = await _loanRepo.GetLoanHistoryByMemberAsync(memberName);
            return loans.Select(l => new LoanHistoryDto
            {
                Id = l.Id,
                LoanDate = l.LoanDate,
                ReturnDate = l.ReturnDate,
                ReturnedAt = l.ReturnedAt,
                BookId = l.BookId,
                BookName = l.Book.Name,
                MemberName = l.MemberName
            }).ToList();
        }

        public async Task<List<LoanHistoryDto>> GetLoanHistoryByBookIdAsync(int bookId)
        {
            var loans = await _loanRepo.GetLoanHistoryByBookIdAsync(bookId);
            return loans.Select(l => new LoanHistoryDto
            {
                Id = l.Id,
                LoanDate = l.LoanDate,
                ReturnDate = l.ReturnDate,
                ReturnedAt = l.ReturnedAt,
                BookId = l.BookId,
                BookName = l.Book.Name,
                MemberName = l.MemberName
            }).ToList();
        }

        public async Task<List<ListLoansDto>> GetActiveLoansAsync()
        {
            var loans = await _loanRepo.GetActiveLoansAsync();
            return loans.Select(l => new ListLoansDto
            {
                Id = l.Id,
                LoanDate = l.LoanDate,
                ReturnDate = l.ReturnDate,
                ReturnedAt = l.ReturnedAt,
                BookId = l.BookId,
                MemberName = l.MemberName
            }).ToList();
        }

        public async Task<List<ListLoansDto>> GetOverdueLoansAsync()
        {
            var loans = await _loanRepo.GetOverdueLoansAsync();
            return loans.Select(l => new ListLoansDto
            {
                Id = l.Id,
                LoanDate = l.LoanDate,
                ReturnDate = l.ReturnDate,
                ReturnedAt = l.ReturnedAt,
                BookId = l.BookId,
                MemberName = l.MemberName
            }).ToList();
        }

        public async Task<LoanHistoryDto?> CreateLoanAsync(CreateLoanDto dto)
        {
            var book = await _bookRepo.GetByIdAsync(dto.BookId);
            if (book == null) return null;

            if (!await _bookRepo.IsAvailableAsync(dto.BookId))
            {
                return null;
            }

            var loan = new Loan
            {
                BookId = dto.BookId,
                MemberName = dto.MemberName,
                LoanDate = DateTime.Now,
                ReturnDate = DateTime.Now.AddDays(14),
                ReturnedAt = null
            };

            await _loanRepo.AddAsync(loan);

            // Decrement book stock
            await _bookRepo.DecrementStockAsync(dto.BookId);

            // Return with book name
            return new LoanHistoryDto
            {
                Id = loan.Id,
                LoanDate = loan.LoanDate,
                ReturnDate = loan.ReturnDate,
                ReturnedAt = loan.ReturnedAt,
                BookId = loan.BookId,
                BookName = book.Name,
                MemberName = loan.MemberName
            };
        }

        public async Task<bool> ReturnLoanAsync(int loanId)
        {
            var loan = await _loanRepo.GetByIdAsync(loanId);
            if (loan == null) return false;

            // Check if already returned
            if (loan.ReturnedAt != null) return false;

            // Return the loan (sets ReturnedAt)
            await _loanRepo.ReturnLoanAsync(loanId);

            // Increment book stock
            await _bookRepo.IncrementStockAsync(loan.BookId);

            return true;
        }
    }
}
