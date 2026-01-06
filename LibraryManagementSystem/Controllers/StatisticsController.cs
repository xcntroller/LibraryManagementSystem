using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        // GET: api/Statistics/most-borrowed?topCount=10
        [HttpGet("most-borrowed")]
        public async Task<ActionResult<List<MostBorrowedBookDto>>> GetMostBorrowedBooks([FromQuery] int topCount = 10)
        {
            if (topCount < 1 || topCount > 100)
                return BadRequest("Top count must be between 1 and 100.");

            var books = await _statisticsService.GetMostBorrowedBooksAsync(topCount);
            return Ok(books);
        }

        // GET: api/Statistics/loans
        [HttpGet("loans")]
        public async Task<ActionResult<LoanStatisticsDto>> GetLoanStatistics()
        {
            var stats = await _statisticsService.GetLoanStatisticsAsync();
            return Ok(stats);
        }

        // GET: api/Statistics/library
        [HttpGet("library")]
        public async Task<ActionResult<LibraryStatisticsDto>> GetLibraryStatistics()
        {
            var stats = await _statisticsService.GetLibraryStatisticsAsync();
            return Ok(stats);
        }
    }
}