using FluentAssertions;
using MovieStore.Application.MovieOperations.Commands.UpdateMovie;
using MovieStore.DBOperations;
using MovieStore.Entities;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.MovieOperations.Commands.UpdateMovie;

public class UpdateMovieCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly MovieStoreDbContext _context;

    public UpdateMovieCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
    }

    [Fact]
    public void WhenGivenMovieIsNotFound_Handle_ThrowsInvalidOperationException()
    {
        // Arrange
        UpdateMovieCommand command = new UpdateMovieCommand(_context);
        command.Id = 999;
        command.Model = new UpdateMovieModel() { Name = "Updated Movie" };

        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("Film bulunamadı.");
    }

    [Fact]
    public void WhenGivenMovieIsNotActive_Handle_ThrowsInvalidOperationException()
    {
        Movie newMovieToTestNotActive = new Movie()
        {
            Name = "test name 1214124124", ReleaseYear = 2000, Price = 15, GenreId = 4, DirectorId = 1, isActive = false
        };
        _context.Movies.Add(newMovieToTestNotActive);
        _context.SaveChanges();
        Movie addedMovieToTestNotActive =
            _context.Movies.SingleOrDefault(movie => movie.Name == newMovieToTestNotActive.Name);
        // Arrange
        UpdateMovieCommand command = new UpdateMovieCommand(_context);
        command.Id = addedMovieToTestNotActive.Id;
        command.Model = new UpdateMovieModel() { Name = "Updated Movie" };

        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("Film bulunamadı.");
    }

    [Fact]
    public void WhenGivenMovieNameAlreadyExistsWithDifferentId_Handle_ThrowsInvalidOperationException()
    {
        // Arrange
        UpdateMovieCommand command = new UpdateMovieCommand(_context);
        command.Id = 3;
        command.Model = new UpdateMovieModel() { Name = "Pulp Fiction" };

        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("Bu isimde bir film zaten var.");
    }

    [Fact]
    public void WhenDefaultInputsAreGiven_Movie_ShouldNotBeChanged()
    {
        // arrange
        Movie newMovie = new Movie()
        {
            Name = "A New Movie",
            Price = 22,
            GenreId = 1,
            DirectorId = 1,
            ReleaseYear = DateTime.Now.AddYears(-5).Year
        };
        _context.Movies.Add(newMovie);
        _context.SaveChanges();
        Movie addedMovie = _context.Movies.SingleOrDefault(movie => movie.Name.ToLower() == newMovie.Name.ToLower());

        UpdateMovieCommand command = new UpdateMovieCommand(_context);
        command.Id = addedMovie.Id;
        UpdateMovieModel model = new UpdateMovieModel()
        {
            Name = "",
            Price = 0,
            GenreId = 0,
            DirectorId = 0,
            ReleaseYear = 0
        };
        command.Model = model;

        Movie movieBeforeUpdate = _context.Movies.SingleOrDefault(movie => movie.Id == command.Id);

        // act
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // assert
        Movie movieAfterUpdate = _context.Movies.SingleOrDefault(movie => movie.Id == command.Id);

        movieAfterUpdate.Should().NotBeNull();
        movieAfterUpdate.Name.Should().Be(movieBeforeUpdate.Name);
        movieAfterUpdate.Price.Should().Be(movieBeforeUpdate.Price);
        movieAfterUpdate.GenreId.Should().Be(movieBeforeUpdate.GenreId);
        movieAfterUpdate.DirectorId.Should().Be(movieBeforeUpdate.DirectorId);
        movieAfterUpdate.ReleaseYear.Should().Be(movieBeforeUpdate.ReleaseYear);
    }

    [Fact]
    public void WhenValidInputsAreGiven_Movie_ShouldBeUpdated()
    {
        // arrange
        UpdateMovieCommand command = new UpdateMovieCommand(_context);
        command.Id = 3;
        UpdateMovieModel model = new UpdateMovieModel()
        {
            Name = "Updated Movie Name",
            Price = 12,
            ReleaseYear = 2000,
            GenreId = 1,
            DirectorId = 2
        };
        command.Model = model;

        // act
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // assert
        Movie movie = _context.Movies.SingleOrDefault(movie => movie.Id == command.Id);

        movie.Should().NotBeNull();
        movie.Name.Should().Be(model.Name);
        movie.Price.Should().Be(model.Price);
        movie.GenreId.Should().Be(model.GenreId);
        movie.DirectorId.Should().Be(model.DirectorId);
        movie.ReleaseYear.Should().Be(model.ReleaseYear);
    }
}