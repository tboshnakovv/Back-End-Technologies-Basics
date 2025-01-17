using MongoDB.Driver;
using MoviesLibraryAPI.Controllers;
using MoviesLibraryAPI.Controllers.Contracts;
using MoviesLibraryAPI.Data.Models;
using MoviesLibraryAPI.Services;
using MoviesLibraryAPI.Services.Contracts;
using System.ComponentModel.DataAnnotations;

namespace MoviesLibraryAPI.XUnitTests
{
    public class XUnitIntegrationTests : IClassFixture<DatabaseFixture>
    {
        private readonly MoviesLibraryXUnitTestDbContext _dbContext;
        private readonly IMoviesLibraryController _controller;
        private readonly IMoviesRepository _repository;

        public XUnitIntegrationTests(DatabaseFixture fixture)
        {
            _dbContext = fixture.DbContext;
            _repository = new MoviesRepository(_dbContext.Movies);
            _controller = new MoviesLibraryController(_repository);

            InitializeDatabaseAsync().GetAwaiter().GetResult();
        }

        private async Task InitializeDatabaseAsync()
        {
            await _dbContext.ClearDatabaseAsync();
        }

        [Fact]
        public async Task AddMovieAsync_WhenValidMovieProvided_ShouldAddToDatabase()
        {
            // Arrange
            var movie = new Movie
            {
                Title = "Test Movie",
                Director = "Test Director",
                YearReleased = 2022,
                Genre = "Action",
                Duration = 120,
                Rating = 7.5
            };

            // Act
            await _controller.AddAsync(movie);

            // Assert
            var resultMovie = await _dbContext.Movies.Find(m => m.Title == "Test Movie").FirstOrDefaultAsync();
            Xunit.Assert.NotNull(resultMovie);
            Xunit.Assert.Equal("Test Movie", resultMovie.Title);
            Xunit.Assert.Equal("Test Director", resultMovie.Director);
            Xunit.Assert.Equal(2022, resultMovie.YearReleased);
            Xunit.Assert.Equal("Action", resultMovie.Genre);
            Xunit.Assert.Equal(120, resultMovie.Duration);
            Xunit.Assert.Equal(7.5, resultMovie.Rating);
        }

        [Fact]
        public async Task AddMovieAsync_WhenInvalidMovieProvided_ShouldThrowValidationException()
        {
            // Arrange
            var invalidMovie = new Movie
            {
                YearReleased = 2022,
                Genre = "Action",
                Duration = 120,
                Rating = 7.5
            };

            // Act and Assert
            var exception = await Xunit.Assert.ThrowsAsync<ValidationException>(() => _controller.AddAsync(invalidMovie));
            Xunit.Assert.Equal("Movie is not valid.", exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_WhenValidTitleProvided_ShouldDeleteMovie()
        {
            var movie = new Movie
            {
                Title = "Test Second Movie",
                Director = "Test Director",
                YearReleased = 2022,
                Genre = "Action",
                Duration = 86,
                Rating = 7.5
            };

            await _controller.AddAsync(movie);

            // Act
            await _controller.DeleteAsync(movie.Title);

            // Assert
            // The movie should no longer exist in the database
            var resultMovie = await _dbContext.Movies.Find(m => m.Title == movie.Title).FirstOrDefaultAsync();
            Xunit.Assert.Null(resultMovie);
        }


        [Xunit.Theory]
        [Xunit.InlineData(null)]
        [Xunit.InlineData("")]
        [Xunit.InlineData("    ")]
        public async Task DeleteAsync_WhenTitleIsNull_ShouldThrowArgumentException(string invalidName)
        {
            // Act and Assert
            Xunit.Assert.ThrowsAsync<ArgumentException>(() => _controller.DeleteAsync(invalidName));
        }


        [Fact]
        public async Task DeleteAsync_WhenTitleDoesNotExist_ShouldThrowInvalidOperationException()
        {
            // Act and Assert
            Xunit.Assert.ThrowsAsync<InvalidDataException>(() => _controller.DeleteAsync("Invalid Name"));
        }

        [Fact]
        public async Task GetAllAsync_WhenNoMoviesExist_ShouldReturnEmptyList()
        {
            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            Xunit.Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAsync_WhenMoviesExist_ShouldReturnAllMovies()
        {
            // Arrange
            var movie = new Movie
            {
                Title = "Get All Second Movie",
                Director = "Test Director",
                YearReleased = 2022,
                Genre = "Action",
                Duration = 86,
                Rating = 7.5
            };



            var secondMovie = new Movie
            {
                Title = "Get All Second Movie",
                Director = "Great Director",
                YearReleased = 2022,
                Genre = "Action",
                Duration = 200,
                Rating = 9.0
            };
            await _dbContext.Movies.InsertManyAsync(new[] { movie, secondMovie });

            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            Xunit.Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByTitle_WhenTitleExists_ShouldReturnMatchingMovie()
        {
            // Arrange
            var Firstmovie = new Movie
            {
                Title = "Get All Second Movie",
                Director = "Test Director",
                YearReleased = 2022,
                Genre = "Action",
                Duration = 86,
                Rating = 7.5
            };



            var secondMovie = new Movie
            {
                Title = "Get All Second Movie",
                Director = "Great Director",
                YearReleased = 2022,
                Genre = "Action",
                Duration = 200,
                Rating = 9.0
            };
            await _dbContext.Movies.InsertManyAsync(new[] { Firstmovie, secondMovie });


            // Act
            var result = await _controller.GetByTitle(Firstmovie.Title);

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(Firstmovie.Title, result.Title);
            Xunit.Assert.Equal(Firstmovie.Director, result.Director);
            Xunit.Assert.Equal(Firstmovie.YearReleased, result.YearReleased);
            Xunit.Assert.Equal(Firstmovie.Genre, result.Genre);
            Xunit.Assert.Equal(Firstmovie.Rating, result.Rating);
        }

        [Fact]
        public async Task GetByTitle_WhenTitleDoesNotExist_ShouldReturnNull()
        {
            // Act
            var resutl = await _controller.GetByTitle("Non existing Title");

            // Assert
            Xunit.Assert.Null(resutl);
        }


        [Fact]
        public async Task SearchByTitleFragmentAsync_WhenTitleFragmentExists_ShouldReturnMatchingMovies()
        {
            // Arrange
            var Firstmovie = new Movie
            {
                Title = "My Search Fragment First Movie",
                Director = "Test Director",
                YearReleased = 2022,
                Genre = "Action",
                Duration = 86,
                Rating = 7.5
            };



            var secondMovie = new Movie
            {
                Title = "Your Search Fragment Second Movie",
                Director = "Great Director",
                YearReleased = 2022,
                Genre = "Action",
                Duration = 200,
                Rating = 9.0
            };
            await _dbContext.Movies.InsertManyAsync(new[] { Firstmovie, secondMovie });

            // Act
            var result = await _controller.SearchByTitleFragmentAsync("Your");


            // Assert // Should return one matching movie
            Xunit.Assert.Equal(1, result.Count());
            var movieResult = result.First();
            Xunit.Assert.Equal(secondMovie.Title, movieResult.Title);
            Xunit.Assert.Equal(secondMovie.Director, movieResult.Director);
            Xunit.Assert.Equal(secondMovie.YearReleased, movieResult.YearReleased);
            Xunit.Assert.Equal(secondMovie.Genre, movieResult.Genre);
            Xunit.Assert.Equal(secondMovie.Duration, movieResult.Duration);
            Xunit.Assert.Equal(secondMovie.Rating, movieResult.Rating);
        }

        [Fact]
        public async Task SearchByTitleFragmentAsync_WhenNoMatchingTitleFragment_ShouldThrowKeyNotFoundException()
        {
            // Act and Assert
            Xunit.Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.SearchByTitleFragmentAsync("NoExistingFragment"));
        }

        [Fact]
        public async Task UpdateAsync_WhenValidMovieProvided_ShouldUpdateMovie()
        {
            // Arrange
            var firstMovie = new Movie
            {
                Title = "First Movie to Update",
                Director = "Test Director",
                YearReleased = 2022,
                Genre = "Action",
                Duration = 86,
                Rating = 7.5
            };



            var secondMovie = new Movie
            {
                Title = "Second Movie to Update",
                Director = "Great Director",
                YearReleased = 2022,
                Genre = "Action",
                Duration = 200,
                Rating = 9.0
            };
            await _dbContext.Movies.InsertManyAsync(new[] { firstMovie, secondMovie });

            // Modify the movie
            var movieToUpdate = await _dbContext.Movies.Find(x => x.Title == firstMovie.Title).FirstOrDefaultAsync();

            movieToUpdate.Title = "Test First Movie to Update UPDATED";
            movieToUpdate.Rating = 10;

            // Act
            await _controller.UpdateAsync(movieToUpdate);


            // Assert
            var updatedMovie = await _dbContext.Movies.Find(x => x.Title == movieToUpdate.Title).FirstOrDefaultAsync();
            Xunit.Assert.NotNull(updatedMovie);
            Xunit.Assert.Equal(movieToUpdate.Rating, updatedMovie.Rating);
        }

        [Fact]
        public async Task UpdateAsync_WhenInvalidMovieProvided_ShouldThrowValidationException()
        {
            // Arrange
            // Movie without required fields
            var invalidMovie = new Movie
            {
                Rating = 10,
            };

            // Act and Assert
            Xunit.Assert.ThrowsAsync<ValidationException>(() => _controller.UpdateAsync(invalidMovie));
        }   
    }
}
