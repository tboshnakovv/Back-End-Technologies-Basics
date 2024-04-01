using LibroConsoleAPI.Business;
using LibroConsoleAPI.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroConsoleAPI.IntegrationTests.XUnit
{
    public class DeleteBookMethodTests : IClassFixture<BookManagerFixture>
    {
        private readonly BookManager _bookManager;
        private readonly TestLibroDbContext _dbContext;

        public DeleteBookMethodTests(BookManagerFixture fixture)
        {
            _bookManager = fixture.BookManager;
            _dbContext = fixture.DbContext;
        }

        [Fact]
        public async Task DeleteBookAsync_ShouldDeleteTheBook()
        {
            //Arrange
            //var newBook = new Book
            //{
            //    Title = "Test Book",
            //    Author = "John Doe",
            //    ISBN = "1234567890123",
            //    YearPublished = 2021,
            //    Genre = "Fiction",
            //    Pages = 100,
            //    Price = 19.99
            //};
            //await _bookManager.AddAsync(newBook);
            await DatabaseSeeder.SeedDatabaseAsync(_dbContext, _bookManager);

            //Act
            await _bookManager.DeleteAsync("9780062315007");
            //Assert
            var bookInDb = _dbContext.Books.ToList();
            Assert.Equal(9, bookInDb.Count);


        }

    }
}
