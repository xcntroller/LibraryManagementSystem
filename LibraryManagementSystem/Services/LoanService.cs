using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Interfaces;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepo;
        private readonly IBookRepository _bookRepo;
        private readonly ILogger<LoanService> _logger;

        public LoanService(ILoanRepository loanRepo, IBookRepository bookRepo, ILogger<LoanService> logger)
        {
            _loanRepo = loanRepo;
            _bookRepo = bookRepo;
            _logger = logger;
        }

        private const int LoanPeriod = 14; // Influences the automatic loan period

        #region Helper methods
        private bool IsLoanOverdue(Loan loan)
        {
            if (loan.ReturnedAt.HasValue) // already returned loans cannot be overdue
                return false;

            return loan.ReturnDate.HasValue && DateTime.Now > loan.ReturnDate.Value;
        }

        private LoanDetailDto MapToLoanDetailDto(Loan loan)
        {
            var isOverdue = IsLoanOverdue(loan);
            string? warningMessage = null;

            if (isOverdue)
            {
                warningMessage = $"⚠️ VAROVÁNÍ: Tato kniha už měla být vrácena! Očekávané datum vrácení: {loan.ReturnDate:dd-MM-yyyy}.";
            }

            return new LoanDetailDto
            {
                Id = loan.Id,
                LoanDate = loan.LoanDate,
                ReturnDate = loan.ReturnDate,
                ReturnedAt = loan.ReturnedAt,
                BookId = loan.BookId,
                MemberName = loan.MemberName,
                IsOverdue = isOverdue,
                WarningMessage = warningMessage
            };
        }

        private ListLoansDto MapToListLoansDto(Loan loan)
        {
            var isOverdue = IsLoanOverdue(loan);

            return new ListLoansDto
            {
                Id = loan.Id,
                LoanDate = loan.LoanDate,
                ReturnDate = loan.ReturnDate,
                ReturnedAt = loan.ReturnedAt,
                BookId = loan.BookId,
                MemberName = loan.MemberName,
                IsOverdue = isOverdue
            };
        }

        #endregion

        #region Query methods
        public async Task<List<ListLoansDto>> GetAllLoansAsync(string? filter)
        {
            var loans = await _loanRepo.GetAllAsync(filter);
            return loans.Select(l => MapToListLoansDto(l)).ToList();
        }

        public async Task<PagedResultDto<ListLoansDto>> GetPagedLoansAsync(int pageNumber, int pageSize, string? filter)
        {
            var (items, totalCount) = await _loanRepo.GetPagedAsync(pageNumber, pageSize, filter);

            return new PagedResultDto<ListLoansDto>
            {
                Items = items.Select(l => MapToListLoansDto(l)).ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<LoanDetailDto?> GetLoanByIdAsync(int id)
        {
            var loan = await _loanRepo.GetByIdAsync(id);
            if (loan == null)
            {
                _logger.LogWarning("Loan with ID {LoanId} not found", id);
                return null;
            }

            return MapToLoanDetailDto(loan);
        }

        public async Task<List<ListLoansDto>> GetActiveLoansByBookId(int id)
        {
            var loans = await _loanRepo.GetActiveLoansByBookIdAsync(id);
            return loans.Select(l => MapToListLoansDto(l)).ToList();
        }

        public async Task<List<ListLoansDto>> GetLoanHistoryByMemberAsync(string memberName)
        {
            var loans = await _loanRepo.GetLoanHistoryByMemberAsync(memberName);
            return loans.Select(l => MapToListLoansDto(l)).ToList();
        }

        public async Task<List<ListLoansDto>> GetLoanHistoryByBookIdAsync(int bookId)
        {
            var loans = await _loanRepo.GetLoanHistoryByBookIdAsync(bookId);
            return loans.Select(l => MapToListLoansDto(l)).ToList();
        }

        public async Task<List<ListLoansDto>> GetActiveLoansAsync()
        {
            var loans = await _loanRepo.GetActiveLoansAsync();
            return loans.Select(l => MapToListLoansDto(l)).ToList();
        }

        public async Task<List<ListLoansDto>> GetOverdueLoansAsync()
        {
            var loans = await _loanRepo.GetOverdueLoansAsync();
            _logger.LogWarning("Found {Count} overdue loans", loans.Count);
            return loans.Select(l => MapToListLoansDto(l)).ToList();
        }

        #endregion

        #region Command methods
        public async Task<ListLoansDto?> CreateLoanAsync(CreateLoanDto dto)
        {
            var book = await _bookRepo.GetByIdAsync(dto.BookId);
            if (book == null)
            {
                _logger.LogWarning("Failed to create loan - Book {BookId} not found", dto.BookId);
                return null;
            }

            if (book.PcsInStock <= 0)
            {
                _logger.LogWarning("Failed to create loan - Book {BookId} not available in stock", dto.BookId);
                return null;
            }

            var loan = new Loan
            {
                BookId = dto.BookId,
                MemberName = dto.MemberName,
                LoanDate = DateTime.Now,
                ReturnDate = DateTime.Now.AddDays(LoanPeriod),
                ReturnedAt = null
            };

            await _loanRepo.AddAsync(loan);
            await _bookRepo.DecrementStockAsync(dto.BookId);

            _logger.LogInformation("Created loan {LoanId} for book {BookId} to member {MemberName}",
                loan.Id, dto.BookId, dto.MemberName);

            return MapToListLoansDto(loan);
        }

        public async Task<bool> ReturnLoanAsync(int loanId)
        {
            var loan = await _loanRepo.GetByIdAsync(loanId);
            if (loan == null)
            {
                _logger.LogWarning("Failed to return loan - Loan {LoanId} not found", loanId);
                return false;
            }

            if (loan.ReturnedAt != null)
            {
                _logger.LogWarning("Failed to return loan {LoanId} - Already returned", loanId);
                return false;
            }

            var wasOverdue = IsLoanOverdue(loan);

            await _loanRepo.ReturnLoanAsync(loanId);
            await _bookRepo.IncrementStockAsync(loan.BookId);

            if (wasOverdue)
            {
                _logger.LogWarning("Loan {LoanId} returned OVERDUE by member {MemberName}",
                    loanId, loan.MemberName);
            }
            else
            {
                _logger.LogInformation("Loan {LoanId} returned by member {MemberName}",
                    loanId, loan.MemberName);
            }

            return true;
        }
        #endregion
    }
}