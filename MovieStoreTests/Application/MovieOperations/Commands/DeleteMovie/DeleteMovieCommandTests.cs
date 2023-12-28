using FluentAssertions;
using MovieStore.Application.MovieOperations.Commands.DeleteMovie;
using MovieStore.DBOperations;
using MovieStore.Entities;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.MovieOperations.Commands.DeleteMovie;

public class DeleteMovieCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly MovieStoreDbContext _context;

    public DeleteMovieCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
    }

    [Fact]
    public void WhenGivenMovieIsNotFound_Handle_ThrowsInvalidOperationException()
    {
        // arrange
        DeleteMovieCommand command = new DeleteMovieCommand(_context);
        command.Id = 999;

        // act & assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("No found movie.");
    }

    [Fact]
    public void WhenGivenMovieIsNotActive_Handle_ThrowsInvalidOperationException()
    {
        Movie newMovieToTestNotActive = new Movie()
        {
            Name = "test name 12346614124124", ReleaseYear = 2000, Price = 15, GenreId = 4, DirectorId = 1,
            isActive = false
        };
        _context.Movies.Add(newMovieToTestNotActive);
        _context.SaveChanges();
        Movie addedMovieToTestNotActive =
            _context.Movies.SingleOrDefault(movie => movie.Name == newMovieToTestNotActive.Name);
        // Arrange
        DeleteMovieCommand command = new DeleteMovieCommand(_context);
        command.Id = addedMovieToTestNotActive.Id;

        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("No found movie.");
    }

    [Fact]
    public void WhenValidInputsAreGiven_Movie_ShouldBeDeleted()
    {
        // arrange
        Movie newMovie = new Movie()
        {
            Name = "A New Movie Name",
            DirectorId = 1,
            GenreId = 1,
            ReleaseYear = 2000,
            Price = 19
        };

        _context.Movies.Add(newMovie);
        _context.SaveChanges();

        Movie createdMovie = _context.Movies.SingleOrDefault(movie => movie.Name == newMovie.Name);

        DeleteMovieCommand command = new DeleteMovieCommand(_context);
        command.Id = createdMovie.Id;

        // act
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // assert
        Movie movie = _context.Movies.SingleOrDefault(movie => movie.Id == createdMovie.Id && movie.isActive);

        movie.Should().BeNull();
    }
}