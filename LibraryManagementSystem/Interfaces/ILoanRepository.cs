using LibraryManagementSystem.Entities;

namespace LibraryManagementSystem.Interfaces
{
    public interface ILoanRepository
    {
        Task<List<Loan>> GetAllAsync(string? filter = null);
        Task<Loan?> GetByIdAsync(int id);
        Task AddAsync(Loan loan);
        Task<List<Loan>> GetActiveLoansByBookIdAsync(int bookId);
        Task<List<Loan>> GetLoanHistoryByMemberAsync(string memberName);
        Task<List<Loan>> GetLoanHistoryByBookIdAsync(int bookId);
        Task<List<Loan>> GetActiveLoansAsync();
        Task<List<Loan>> GetOverdueLoansAsync();
        Task ReturnLoanAsync(int loanId);
    }
}