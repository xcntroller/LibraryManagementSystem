using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Entities;

namespace LibraryManagementSystem.Services
{
    public class BookService
    {
        private readonly IBookRepository _bookRepo;
        private readonly IAuthorRepository _authorRepo;

        public BookService(IBookRepository bookRepo, IAuthorRepository authorRepo)
        {
            _bookRepo = bookRepo;
            _authorRepo = authorRepo;
        }

        public async Task<List<ListBookDto>> GetAllBooksAsync(string? filter)
        {
            var books = await _bookRepo.GetAllAsync(filter);
            return books.Select(b => new ListBookDto
            {
                Id = b.Id,
                Name = b.Name,
                PublicationYear = b.PublicationYear,
                PcsInStock = b.PcsInStock,
                AuthorId = b.AuthorId,
            }).ToList();
        }

        public async Task<BookDetailDto?> GetBookByIdAsync(int id)
        {
            var book = await _bookRepo.GetByIdAsync(id);
            if (book == null) return null;

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

        public async Task<BookDetailDto?> GetBookByISBNAsync(string isbn)
        {
            var book = await _bookRepo.GetByISBNAsync(isbn);
            if (book == null) return null;

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

        public async Task<BookDetailDto?> CreateBookAsync(CreateBookDto dto)
        {
            // Validate author exists
            if (!await _authorRepo.ExistsByIdAsync(dto.AuthorId))
            {
                throw new Exception("Autor neexistuje.");
            }

            var book = new Book
            {
                Name = dto.Name,
                ISBN = dto.ISBN,
                Description = dto.Description,
                PublicationYear = dto.PublicationYear,
                PcsTotal = dto.PcsTotal,
                PcsInStock = dto.PcsTotal,
                AuthorId = dto.AuthorId
            };

            await _bookRepo.AddAsync(book);

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

        public async Task<bool> UpdateBookAsync(int id, UpdateBookDto dto)
        {
            var book = await _bookRepo.GetByIdAsync(id);
            if (book == null) return false;

            // Validate author exists
            if (!await _authorRepo.ExistsByIdAsync(dto.AuthorId))
            {
                throw new Exception("Autor neexistuje.");
            }

            book.Name = dto.Name;
            book.ISBN = dto.ISBN;
            book.Description = dto.Description;
            book.PublicationYear = dto.PublicationYear;
            book.PcsTotal = dto.PcsTotal;
            book.AuthorId = dto.AuthorId;

            await _bookRepo.UpdateAsync(book);
            return true;
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _bookRepo.GetByIdAsync(id);
            if (book == null) return false;

            // Check if book has active loans
            if (await _bookRepo.HasActiveLoansAsync(id))
            {
                throw new Exception("Nelze smazat knihu která je již půjčená.");
            }

            await _bookRepo.DeleteAsync(book);
            return true;
        }

        public async Task<bool> IsBookAvailableAsync(int id)
        {
            return await _bookRepo.IsAvailableAsync(id);
        }

        public async Task DecrementStockAsync(int bookId)
        {
            await _bookRepo.DecrementStockAsync(bookId);
        }

        public async Task IncrementStockAsync(int bookId)
        {
            await _bookRepo.IncrementStockAsync(bookId);
        }
    }
}