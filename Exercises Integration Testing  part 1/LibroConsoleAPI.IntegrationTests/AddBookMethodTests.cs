using LibroConsoleAPI.Business;
using LibroConsoleAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroConsoleAPI.IntegrationTests.XUnit
{
    public class AddBookMethodTests : IClassFixture<BookManagerFixture>
    {
        private readonly BookManager _bookManager;
        private readonly TestLibroDbContext _dbContext;

        public AddBookMethodTests(BookManagerFixture fixture)
        {
            _bookManager = fixture.BookManager;
            _dbContext = fixture.DbContext;
        }

        // Possitive Test
        [Fact]
        public async Task AddBookAsync_ShouldAddBook()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Test Book",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            await _bookManager.AddAsync(newBook);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == newBook.ISBN);
            Assert.NotNull(bookInDb);
            Assert.Equal(newBook.Title, bookInDb.Title);
            Assert.Equal(newBook.Author, bookInDb.Author);
        }

        // Negative test
        [Fact]
        public async Task AddBookAsync_WhenPassInvalidTitle_ShoulThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = new string('A', 500),
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(newBook));

            // Assert
            Assert.Equal("Book is invalid.", exception.Result.Message);
        }

        // Negative test
        [Fact]
        public async Task AddBookAsync_WhenPassInvalidISBN_ShoulThrowException()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Some book title",
                Author = "John Doe",
                ISBN = "invalid",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(newBook));

            // Assert
            Assert.Equal("Book is invalid.", exception.Result.Message);
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync();
            Assert.Null(bookInDb);
        }
    }
}
