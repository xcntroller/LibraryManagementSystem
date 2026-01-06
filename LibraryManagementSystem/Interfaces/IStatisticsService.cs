using LibraryManagementSystem.DTOs;

namespace LibraryManagementSystem.Interfaces
{
    public interface IStatisticsService
    {
        Task<List<MostBorrowedBookDto>> GetMostBorrowedBooksAsync(int topCount = 10);
        Task<LoanStatisticsDto> GetLoanStatisticsAsync();
        Task<LibraryStatisticsDto> GetLibraryStatisticsAsync();
    }
}