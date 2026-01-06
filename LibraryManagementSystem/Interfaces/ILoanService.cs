using LibraryManagementSystem.DTOs;

namespace LibraryManagementSystem.Interfaces
{
    public interface ILoanService
    {
        Task<List<ListLoansDto>> GetAllLoansAsync(string? filter);
        Task<PagedResultDto<ListLoansDto>> GetPagedLoansAsync(int pageNumber, int pageSize, string? filter);
        Task<LoanDetailDto?> GetLoanByIdAsync(int id);
        Task<List<ListLoansDto>> GetActiveLoansByBookId(int id);
        Task<List<ListLoansDto>> GetLoanHistoryByMemberAsync(string memberName);
        Task<List<ListLoansDto>> GetLoanHistoryByBookIdAsync(int bookId);
        Task<List<ListLoansDto>> GetActiveLoansAsync();
        Task<List<ListLoansDto>> GetOverdueLoansAsync();
        Task<ListLoansDto?> CreateLoanAsync(CreateLoanDto dto);
        Task<bool> ReturnLoanAsync(int loanId);

    }
}
