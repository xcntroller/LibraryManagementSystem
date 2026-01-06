using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoansController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        // GET: api/Loans
        [HttpGet]
        public async Task<ActionResult> GetAllLoans(
            [FromQuery] int? pageNumber = null,
            [FromQuery] int? pageSize = null,
            [FromQuery] string? filter = null)
        {
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                    return BadRequest("Page number must be >= 1 and page size must be between 1 and 100.");

                var pagedLoans = await _loanService.GetPagedLoansAsync(pageNumber.Value, pageSize.Value, filter);
                return Ok(pagedLoans);
            }

            var loans = await _loanService.GetAllLoansAsync(filter);
            return Ok(loans);
        }

        // GET: api/Loans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoanDetailDto>> GetLoanById(int id)
        {
            var loan = await _loanService.GetLoanByIdAsync(id);
            if (loan == null)
                return NotFound($"Loan with ID {id} not found.");

            return Ok(loan);
        }

        // GET: api/Loans/books/5/active
        // this might be redundant
        [HttpGet("books/{id}/active")]
        public async Task<ActionResult<List<ListLoansDto>>> GetActiveLoansByBookId(int id)
        {
            var loans = await _loanService.GetActiveLoansByBookId(id);
            return Ok(loans);
        }


        // GET: api/Loans/member?name=JohnDoe
        // this is not great, if there was auth i'd use IDs 
        [HttpGet("member")]
        public async Task<ActionResult<List<ListLoansDto>>> GetLoanHistoryByPerson(string memberName) 
        {
            var history = await _loanService.GetLoanHistoryByMemberAsync(memberName);
            if (history == null) return NotFound($"{memberName} didn't loan any books.");
            return Ok(history);
        }

        // GET: api/Loans/books/5
        [HttpGet("/books/{id}")]
        public async Task<ActionResult<List<ListLoansDto>>> GetLoanHistoryByBookId(int id)
        {
            var history = await _loanService.GetLoanHistoryByBookIdAsync(id);
            if (history == null) return NotFound($"This book doesn't exist.");
            return Ok(history);
        }

        // GET: api/Loans/active
        [HttpGet("active")]
        public async Task<ActionResult<List<ListLoansDto>>> GetActiveLoans()
        {
           var loans = await _loanService.GetActiveLoansAsync();
            return Ok(loans);
        }

        // GET: api/Loans/overdue
        [HttpGet("overdue")]
        public async Task<ActionResult<List<ListLoansDto>>> GetOverdueLoans()
        {
            var loans = await _loanService.GetOverdueLoansAsync();
            return Ok(loans);
        }

        // POST: api/Loans/add
        [HttpPost("add")]
        public async Task<ActionResult<LoanDetailDto>> CreateLoan([FromBody] CreateLoanDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var loan = await _loanService.CreateLoanAsync(dto);
            if (loan == null)
                return BadRequest("Book not found or not available.");

            return CreatedAtAction(nameof(GetLoanById), new { id = loan.Id }, loan);
        }

        // PUT: api/Loans/5/return
        [HttpPut("{id}/return")]
        public async Task<IActionResult> ReturnLoan(int id)
        {
            var success = await _loanService.ReturnLoanAsync(id);
            if (!success)
                return BadRequest("Loan not found or already returned.");

            return NoContent();
        }
    }
}
