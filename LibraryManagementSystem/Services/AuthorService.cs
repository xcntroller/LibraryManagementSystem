using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Entities;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepo;
        private readonly ILogger<AuthorService> _logger;

        public AuthorService(IAuthorRepository authorRepo, ILogger<AuthorService> logger)
        {
            _authorRepo = authorRepo;
            _logger = logger;
        }

        #region Helper Methods

        private static ListAuthorDto MapToListAuthorDto(Author author)
        {
            return new ListAuthorDto
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                BirthYear = author.BirthYear
            };
        }

        private static AuthorDetailDto MapToAuthorDetailDto(Author author)
        {
            return new AuthorDetailDto
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Description = author.Description,
                BirthYear = author.BirthYear,
                Books = author.Books.Select(b => new ListBookDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    PublicationYear = b.PublicationYear,
                    PcsInStock = b.PcsInStock,
                    AuthorId = b.AuthorId
                }).ToList()
            };
        }

        #endregion

        #region Query Methods

        public async Task<List<ListAuthorDto>> GetAllAuthorsAsync(string? filter)
        {
            var authors = await _authorRepo.GetAllAsync(filter);
            return authors.Select(a => MapToListAuthorDto(a)).ToList();
        }

        public async Task<PagedResultDto<ListAuthorDto>> GetPagedAuthorsAsync(int pageNumber, int pageSize, string? filter)
        {
            var (items, totalCount) = await _authorRepo.GetPagedAsync(pageNumber, pageSize, filter);

            return new PagedResultDto<ListAuthorDto>
            {
                Items = items.Select(a => MapToListAuthorDto(a)).ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<AuthorDetailDto?> GetAuthorByIdAsync(int id)
        {
            var author = await _authorRepo.GetByIdAsync(id);
            if (author == null)
            {
                _logger.LogWarning("Author with ID {AuthorId} not found", id);
                return null;
            }

            return MapToAuthorDetailDto(author);
        }

        public async Task<List<ListBookDto>> GetAuthorsBooksAsync(int authorId)
        {
            var books = await _authorRepo.GetAuthorsBooksAsync(authorId);

            return books.Select(b => new ListBookDto
            {
                Id = b.Id,
                Name = b.Name,
                PublicationYear = b.PublicationYear,
                PcsInStock = b.PcsInStock,
                AuthorId = b.AuthorId
            }).ToList();
        }

        public async Task<bool> AuthorExistsByIdAsync(int id)
        {
            return await _authorRepo.ExistsByIdAsync(id);
        }

        #endregion

        #region Command Methods

        public async Task<AuthorDetailDto?> CreateAuthorAsync(CreateAuthorDto dto)
        {
            var author = new Author
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Description = dto.Description,
                BirthYear = dto.BirthYear,
            };

            await _authorRepo.AddAsync(author);

            _logger.LogInformation("Created author {AuthorId} ({FirstName} {LastName})",
                author.Id, author.FirstName, author.LastName);

            return new AuthorDetailDto
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Description = author.Description,
                BirthYear = author.BirthYear,
                Books = []
            };
        }

        public async Task<bool> UpdateAuthorAsync(int id, UpdateAuthorDto dto)
        {
            var author = await _authorRepo.GetByIdAsync(id);
            if (author == null)
            {
                _logger.LogWarning("Failed to update - Author {AuthorId} not found", id);
                return false;
            }

            author.FirstName = dto.FirstName;
            author.LastName = dto.LastName;
            author.Description = dto.Description ?? author.Description;
            author.BirthYear = dto.BirthYear;

            await _authorRepo.UpdateAsync(author);

            _logger.LogInformation("Updated author {AuthorId} ({FirstName} {LastName})",
                id, author.FirstName, author.LastName);
            return true;
        }

        public async Task<bool> DeleteAuthorAsync(int id)
        {
            var author = await _authorRepo.GetByIdAsync(id);
            if (author == null)
            {
                _logger.LogWarning("Failed to delete - Author {AuthorId} not found", id);
                return false;
            }

            if (await _authorRepo.HasBooksAsync(id))
            {
                _logger.LogWarning("Failed to delete author {AuthorId} ({FirstName} {LastName}) - has assigned books",
                    id, author.FirstName, author.LastName);
                return false;
            }

            await _authorRepo.DeleteAsync(author);

            _logger.LogInformation("Deleted author {AuthorId} ({FirstName} {LastName})",
                id, author.FirstName, author.LastName);
            return true;
        }

        #endregion
    }
}