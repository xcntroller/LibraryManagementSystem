using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult> GetAllAuthors(
            [FromQuery] int? pageNumber = null,
            [FromQuery] int? pageSize = null,
            [FromQuery] string? filter = null)
        {
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                    return BadRequest("Page number must be >= 1 and page size must be between 1 and 100.");

                var pagedAuthors = await _authorService.GetPagedAuthorsAsync(pageNumber.Value, pageSize.Value, filter);
                return Ok(pagedAuthors);
            }

            var authors = await _authorService.GetAllAuthorsAsync(filter);
            return Ok(authors);
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDetailDto>> GetAuthorById(int id)
        {
            var author = await _authorService.GetAuthorByIdAsync(id);
            if (author == null) return NotFound($"Author with ID {id} not found.");

            return Ok(author);
        }

        // GET: api/Authors/5/books
        [HttpGet("{id}/books")]
        public async Task<ActionResult<List<ListBookDto>>> GetAuthorBooks(int id)
        {
            if (!await _authorService.AuthorExistsByIdAsync(id))
                return NotFound($"Author with ID {id} not found.");

            var books = await _authorService.GetAuthorsBooksAsync(id);
            return Ok(books);
        }

        // POST: api/Authors/add
        [HttpPost("add")]
        public async Task<ActionResult<AuthorDetailDto>> CreateAuthor([FromBody] CreateAuthorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var author = await _authorService.CreateAuthorAsync(dto);
            if (author == null)
                return BadRequest("Failed to create author.");

            return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, author);
        }

        // PUT: api/Authors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] UpdateAuthorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _authorService.UpdateAuthorAsync(id, dto);
            if (!success)
                return NotFound($"Author with ID {id} not found.");

            return NoContent();
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var success = await _authorService.DeleteAuthorAsync(id);
            if (!success)
                return BadRequest("Cannot delete author with assigned books or author not found.");

            return NoContent();
        }
    }
}
