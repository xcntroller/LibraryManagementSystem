using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<List<ListBookDto>>> GetAllBooks([FromQuery] string? filter = null)
        {
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

        // POST: api/Books/add
        [HttpPost("add")]
        public async Task<ActionResult<BookDetailDto>> CreateBook([FromBody] CreateBookDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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
    }
}