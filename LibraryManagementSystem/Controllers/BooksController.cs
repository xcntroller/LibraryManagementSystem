using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<PagedResultDto<ListBookDto>>> GetAllBooks(
            [FromQuery] int? pageNumber = null,
            [FromQuery] int? pageSize = null,
            [FromQuery] string? filter = null)
        {
            // If pagination parameters are provided, return paged results
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                    return BadRequest("Page number must be >= 1 and page size must be between 1 and 100.");

                var pagedBooks = await _bookService.GetPagedBooksAsync(pageNumber.Value, pageSize.Value, filter);
                return Ok(pagedBooks);
            }

            // Otherwise return all books
            var books = await _bookService.GetAllBooksAsync(filter);
            return Ok(books);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailDto>> GetBookById(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
                return NotFound($"Book with ID {id} not found.");

            return Ok(book);
        }

        // GET: api/Books/isbn/9780123456789
        [HttpGet("isbn/{isbn}")]
        public async Task<ActionResult<BookDetailDto>> GetBookByISBN(string isbn)
        {
            var book = await _bookService.GetBookByISBNAsync(isbn);
            if (book == null)
                return NotFound($"Book with ISBN {isbn} not found.");

            return Ok(book);
        }

        // GET: api/Books/5/available
        [HttpGet("{id}/available")]
        public async Task<ActionResult<bool>> IsBookAvailable(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
                return NotFound($"Book with ID {id} not found.");

            return Ok(new { bookId = id, isAvailable = book.PcsInStock > 0 });
        }

        // GET: api/Books/author/5
        [HttpGet("author/{id}")]
        public async Task<ActionResult<List<ListBookDto>>> GetBooksByAuthor(int id)
        {
            var books = await _bookService.GetBooksByAuthorIdAsync(id);
            return Ok(books);
        }

        // POST: api/Books/add
        [HttpPost("add")]
        public async Task<ActionResult<BookDetailDto>> CreateBook([FromBody] CreateBookDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!IsValidISBN(dto.ISBN))
                return BadRequest("Invalid ISBN format. Please provide a valid ISBN-10 or ISBN-13.");


            var book = await _bookService.CreateBookAsync(dto);
            if (book == null)
                return BadRequest("Author does not exist.");

            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!IsValidISBN(dto.ISBN))
                return BadRequest("Invalid ISBN format. Please provide a valid ISBN-10 or ISBN-13.");

            var success = await _bookService.UpdateBookAsync(id, dto);
            if (!success)
                return NotFound($"Book with ID {id} not found or author does not exist.");

            return NoContent();
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var success = await _bookService.DeleteBookAsync(id);
            if (!success)
                return BadRequest("Cannot delete book with active loans or book not found.");

            return NoContent();
        }

        #region ISBN Validity check
        private bool IsValidISBN(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                return false;

            isbn = isbn.Replace("-", "").Replace(" ", "");

            // Check if it's ISBN-10 or ISBN-13
            if (isbn.Length == 10)
                return IsValidISBN10(isbn);
            else if (isbn.Length == 13)
                return IsValidISBN13(isbn);

            return false;
        }

        private bool IsValidISBN10(string isbn)
        {
            if (!isbn.All(c => char.IsDigit(c) || c == 'X' || c == 'x'))
                return false;

            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                if (!char.IsDigit(isbn[i]))
                    return false;
                sum += (isbn[i] - '0') * (10 - i);
            }

            char lastChar = isbn[9];
            int checkDigit = lastChar == 'X' || lastChar == 'x' ? 10 : lastChar - '0';
            sum += checkDigit;

            return sum % 11 == 0;
        }

        private bool IsValidISBN13(string isbn)
        {
            if (!isbn.All(char.IsDigit))
                return false;

            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int digit = isbn[i] - '0';
                sum += (i % 2 == 0) ? digit : digit * 3;
            }

            int checkDigit = isbn[12] - '0';
            int calculatedCheckDigit = (10 - (sum % 10)) % 10;

            return checkDigit == calculatedCheckDigit;
        }
        #endregion
    }
}