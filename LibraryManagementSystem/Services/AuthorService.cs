using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Entities;

namespace LibraryManagementSystem.Services
{
    public class AuthorService
    {
        private readonly IAuthorRepository _authorRepo;

        public AuthorService(IAuthorRepository authorRepo)
        {
            _authorRepo = authorRepo;
        }

        public async Task<List<ListAuthorDto>> GetAllAuthorsAsync(string? filter)
        {
            var authors = await _authorRepo.GetAllAsync(filter);
            return authors.Select(a => new ListAuthorDto
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                BirthYear = a.BirthYear,
            }).ToList();
        }

        public async Task<AuthorDetailDto?> GetAuthorByIdAsync(int id)
        {
            var author = await _authorRepo.GetByIdAsync(id);
            if (author == null) return null;

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

            return new AuthorDetailDto
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Description = author.Description,
                BirthYear = author.BirthYear,
                Books = new List<ListBookDto>()
            };
        }

        public async Task<bool> UpdateAuthorAsync(int id, UpdateAuthorDto dto)
        {
            var author = await _authorRepo.GetByIdAsync(id);
            if (author == null) return false;

            author.FirstName = dto.FirstName;
            author.LastName = dto.LastName;
            author.Description = dto.Description ?? author.Description;
            author.BirthYear = dto.BirthYear;

            await _authorRepo.UpdateAsync(author);
            return true;
        }

        public async Task<bool> DeleteAuthorAsync(int id)
        {
            var author = await _authorRepo.GetByIdAsync(id);
            if (author == null) return false;

            if (await _authorRepo.HasBooksAsync(id))
            {
                return false; // Cannot delete author with books
            }

            await _authorRepo.DeleteAsync(author);
            return true;
        }
    }
}