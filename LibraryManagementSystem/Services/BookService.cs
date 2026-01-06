using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Entities;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepo;
        private readonly IAuthorRepository _authorRepo;
        private readonly ILogger<BookService> _logger;

        public BookService(IBookRepository bookRepo, IAuthorRepository authorRepo, ILogger<BookService> logger)
        {
            _bookRepo = bookRepo;
            _authorRepo = authorRepo;
            _logger = logger;
        }

        #region Helper Methods

        private static BookDetailDto MapToBookDetailDto(Book book)
        {
            return new BookDetailDto
            {
                Id = book.Id,
                Name = book.Name,
                ISBN = book.ISBN,
                Description = book.Description,
                PublicationYear = book.PublicationYear,
                PcsTotal = book.PcsTotal,
                PcsInStock = book.PcsInStock,
                AuthorId = book.AuthorId
            };
        }

        private static ListBookDto MapToListBookDto(Book book)
        {
            return new ListBookDto
            {
                Id = book.Id,
                Name = book.Name,
                PublicationYear = book.PublicationYear,
                PcsInStock = book.PcsInStock,
                AuthorId = book.AuthorId
            };
        }

        #endregion

        #region Query Methods

        public async Task<List<ListBookDto>> GetAllBooksAsync(string? filter)
        {
            var books = await _bookRepo.GetAllAsync(filter);
            return books.Select(b => MapToListBookDto(b)).ToList();
        }

        public async Task<PagedResultDto<ListBookDto>> GetPagedBooksAsync(int pageNumber, int pageSize, string? filter)
        {
            var (items, totalCount) = await _bookRepo.GetPagedAsync(pageNumber, pageSize, filter);

            return new PagedResultDto<ListBookDto>
            {
                Items = items.Select(b => MapToListBookDto(b)).ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<BookDetailDto?> GetBookByIdAsync(int id)
        {
            var book = await _bookRepo.GetByIdAsync(id);
            if (book == null)
            {
                _logger.LogWarning("Book with ID {BookId} not found", id);
                return null;
            }

            return MapToBookDetailDto(book);
        }

        public async Task<BookDetailDto?> GetBookByISBNAsync(string isbn)
        {
            var book = await _bookRepo.GetByISBNAsync(isbn);
            if (book == null)
            {
                _logger.LogWarning("Book with ISBN {ISBN} not found", isbn);
                return null;
            }

            return MapToBookDetailDto(book);
        }

        public async Task<List<ListBookDto>> GetBooksByAuthorIdAsync(int authorId)
        {
            var books = await _bookRepo.GetByAuthorIdAsync(authorId);
            return books.Select(b => MapToListBookDto(b)).ToList();
        }

        #endregion

        #region Command Methods

        public async Task<BookDetailDto?> CreateBookAsync(CreateBookDto dto)
        {
            if (!await _authorRepo.ExistsByIdAsync(dto.AuthorId))
            {
                _logger.LogWarning("Failed to create book - Author {AuthorId} does not exist", dto.AuthorId);
                return null;
            }

            var book = new Book
            {
                Name = dto.Name,
                ISBN = dto.ISBN,
                Description = dto.Description,
                PublicationYear = dto.PublicationYear,
                PcsTotal = dto.PcsTotal,
                PcsInStock = dto.PcsTotal, // Initially all copies are in stock
                AuthorId = dto.AuthorId
            };

            await _bookRepo.AddAsync(book);

            _logger.LogInformation("Created book {BookId} ({BookName}) with {Stock} copies",
                book.Id, book.Name, book.PcsTotal);

            return MapToBookDetailDto(book);
        }

        public async Task<bool> UpdateBookAsync(int id, UpdateBookDto dto)
        {
            var book = await _bookRepo.GetByIdAsync(id);
            if (book == null)
            {
                _logger.LogWarning("Failed to update - Book {BookId} not found", id);
                return false;
            }

            if (!await _authorRepo.ExistsByIdAsync(dto.AuthorId))
            {
                _logger.LogWarning("Failed to update book {BookId} - Author {AuthorId} does not exist", id, dto.AuthorId);
                return false;
            }

            book.Name = dto.Name;
            book.ISBN = dto.ISBN;
            book.Description = dto.Description;
            book.PublicationYear = dto.PublicationYear;
            book.PcsTotal = dto.PcsTotal;
            book.AuthorId = dto.AuthorId;

            await _bookRepo.UpdateAsync(book);

            _logger.LogInformation("Updated book {BookId} ({BookName})", id, book.Name);
            return true;
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _bookRepo.GetByIdAsync(id);
            if (book == null)
            {
                _logger.LogWarning("Failed to delete - Book {BookId} not found", id);
                return false;
            }

            if (await _bookRepo.HasActiveLoansAsync(id))
            {
                _logger.LogWarning("Failed to delete book {BookId} ({BookName}) - has active loans", id, book.Name);
                return false;
            }

            await _bookRepo.DeleteAsync(book);

            _logger.LogInformation("Deleted book {BookId} ({BookName})", id, book.Name);
            return true;
        }

        #endregion

        #region Stock Management

        public async Task DecrementStockAsync(int bookId)
        {
            await _bookRepo.DecrementStockAsync(bookId);
        }

        public async Task IncrementStockAsync(int bookId)
        {
            await _bookRepo.IncrementStockAsync(bookId);
        }

        #endregion
    }
}