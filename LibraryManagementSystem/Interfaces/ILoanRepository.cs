using LibraryManagementSystem.Entities;

namespace LibraryManagementSystem.Interfaces
{
    public interface ILoanRepository
    {
        Task<List<Loan>> GetAllAsync(string? filter = null);
        Task<(List<Loan> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? filter = null);
        Task<Loan?> GetByIdAsync(int id);
        Task AddAsync(Loan loan);
        Task<List<Loan>> GetActiveLoansByBookIdAsync(int bookId);
        Task<List<Loan>> GetLoanHistoryByMemberAsync(string memberName);
        Task<List<Loan>> GetLoanHistoryByBookIdAsync(int bookId);
        Task<List<Loan>> GetActiveLoansAsync();
        Task<List<Loan>> GetOverdueLoansAsync();
        Task ReturnLoanAsync(int loanId);

        // statistics
        Task<List<(int BookId, string BookName, string ISBN, int BorrowCount, int AuthorId, string AuthorName)>> GetMostBorrowedBooksAsync(int topCount = 10);
        Task<double> GetAverageLoanDurationAsync();
        Task<int> GetTotalLoansCountAsync();
        Task<int> GetCompletedLoansCountAsync();
        Task<int> GetUniqueBorrowersCountAsync();
    }
}