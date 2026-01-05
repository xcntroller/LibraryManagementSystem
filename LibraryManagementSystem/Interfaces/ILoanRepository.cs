using LibraryManagementSystem.Entities;

namespace LibraryManagementSystem.Interfaces
{
    public interface ILoanRepository
    {
        Task<List<Loan>> GetAllAsync(string? filter = null);
        Task<Loan?> GetByIdAsync(int id);
        Task AddAsync(Loan loan);
        Task UpdateAsync(Loan loan);
        Task<bool> IsOverdueAsync(int id);
        Task<List<Loan>> GetActiveLoansByBookIdAsync(int bookId);
        Task<List<Loan>> GetLoansByMemberAsync(string memberName);
        Task<List<Loan>> GetActiveLoansAsync();
        Task<List<Loan>> GetOverdueLoansAsync();
        Task ReturnLoanAsync(int loanId);
    }
}