using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Interfaces;

namespace LibraryManagementSystem.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ILoanRepository _loanRepo;
        private readonly IBookRepository _bookRepo;
        private readonly IAuthorRepository _authorRepo;

        public StatisticsService(ILoanRepository loanRepo, IBookRepository bookRepo, IAuthorRepository authorRepo)
        {
            _loanRepo = loanRepo;
            _bookRepo = bookRepo;
            _authorRepo = authorRepo;
        }

        private const int MostBorrowedTopCount = 10; // This changes how many books the function GetMostBorrowedBooksAsync returns

        public async Task<List<MostBorrowedBookDto>> GetMostBorrowedBooksAsync(int topCount = MostBorrowedTopCount)
        {
            var mostBorrowed = await _loanRepo.GetMostBorrowedBooksAsync(topCount);

            return mostBorrowed.Select(mb => new MostBorrowedBookDto
            {
                BookId = mb.BookId,
                BookName = mb.BookName,
                ISBN = mb.ISBN,
                BorrowCount = mb.BorrowCount,
                AuthorId = mb.AuthorId,
                AuthorName = mb.AuthorName
            }).ToList();
        }

        public async Task<LoanStatisticsDto> GetLoanStatisticsAsync()
        {
            var totalLoans = await _loanRepo.GetTotalLoansCountAsync();
            var completedLoans = await _loanRepo.GetCompletedLoansCountAsync();
            var activeLoans = await _loanRepo.GetActiveLoansAsync();
            var overdueLoans = await _loanRepo.GetOverdueLoansAsync();
            var averageDuration = await _loanRepo.GetAverageLoanDurationAsync();

            return new LoanStatisticsDto
            {
                TotalLoans = totalLoans,
                CompletedLoans = completedLoans,
                ActiveLoans = activeLoans.Count,
                OverdueLoans = overdueLoans.Count,
                AverageLoanDurationDays = Math.Round(averageDuration, 2)
            };
        }

        public async Task<LibraryStatisticsDto> GetLibraryStatisticsAsync()
        {
            var totalBooks = await _bookRepo.GetTotalBooksCountAsync();
            var totalAuthors = await _authorRepo.GetTotalAuthorsCountAsync();
            var availableBooks = await _bookRepo.GetAvailableBooksCountAsync();
            var uniqueBorrowers = await _loanRepo.GetUniqueBorrowersCountAsync();
            var mostBorrowed = await GetMostBorrowedBooksAsync(5);
            var loanStats = await GetLoanStatisticsAsync();

            return new LibraryStatisticsDto
            {
                TotalBooks = totalBooks,
                TotalAuthors = totalAuthors,
                TotalAvailableBooks = availableBooks,
                UniqueBorrowers = uniqueBorrowers,
                MostBorrowedBooks = mostBorrowed,
                LoanStatistics = loanStats
            };
        }
    }
}