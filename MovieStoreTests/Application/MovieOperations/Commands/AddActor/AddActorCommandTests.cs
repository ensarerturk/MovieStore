using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MovieStore.Application.MovieOperations.Commands.AddActor;
using MovieStore.DBOperations;
using MovieStore.Entities;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.MovieOperations.Commands.AddActor;

public class AddActorCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly MovieStoreDbContext _dbContext;

    public AddActorCommandTests(CommonTestFixture testFixture)
    {
        _dbContext = testFixture.Context;
    }

    [Fact]
    public void WhenGivenMovieIsNotFound_Handle_ThrowsInvalidOperationException()
    {
        // Arrange
        AddActorCommand command = new AddActorCommand(_dbContext);
        command.Id = 999;
        command.Model = new AddActorModel() { ActorId = 1 };

        // Act & Assert
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
            Name = "test name 12545545414124124", ReleaseYear = 2000, Price = 15, GenreId = 4, DirectorId = 1,
            isActive = false
        };
        _dbContext.Movies.Add(newMovieToTestNotActive);
        _dbContext.SaveChanges();
        Movie addedMovieToTestNotActive =
            _dbContext.Movies.SingleOrDefault(movie => movie.Name == newMovieToTestNotActive.Name);
        // Arrange
        AddActorCommand command = new AddActorCommand(_dbContext);
        command.Id = addedMovieToTestNotActive.Id;
        command.Model = new AddActorModel() { ActorId = 3 };

        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("No found movie.");
    }

    [Fact]
    public void WhenGivenActorIsNotFound_Handle_ThrowsInvalidOperationException()
    {
        // Arrange
        AddActorCommand command = new AddActorCommand(_dbContext);
        command.Id = 1;
        command.Model = new AddActorModel() { ActorId = 999 };

        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("No found actor.");
    }

    [Fact]
    public void WhenGivenActorIsAlreadyPlayingInGivenMovie_Handle_ThrowsInvalidOperationException()
    {
        // Arrange

        Actor actor = new Actor() { FirstName = "A New", LastName = "Actor To Test" };
        _dbContext.Actors.Add(actor);
        _dbContext.SaveChanges();
        Actor addedActor = _dbContext.Actors.Include(actor => actor.Movies).SingleOrDefault(a =>
            a.FirstName.ToLower() == actor.FirstName.ToLower() && a.LastName.ToLower() == actor.LastName.ToLower());

        Movie movie = new Movie()
            { Name = "A New Movie To Test", Price = 15, ReleaseYear = 2000, GenreId = 1, DirectorId = 1 };
        _dbContext.Movies.Add(movie);
        _dbContext.SaveChanges();
        Movie addedMovie = _dbContext.Movies.SingleOrDefault(m => m.Name.ToLower() == movie.Name.ToLower());

        addedActor.Movies.Add(addedMovie);
        _dbContext.SaveChanges();

        AddActorCommand command = new AddActorCommand(_dbContext);
        command.Id = addedMovie.Id;
        command.Model = new AddActorModel() { ActorId = addedActor.Id };

        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("Bu oyuncu zaten bu filmde oynuyor.");
    }

    [Fact]
    public void WhenValidInputsAreGivenMovie_Actor_ShouldBeAddedToTheMovie()
    {
        Actor actor = new Actor() { FirstName = "A New", LastName = "Actor To Test Add Actor" };
        _dbContext.Actors.Add(actor);
        _dbContext.SaveChanges();
        Actor addedActor = _dbContext.Actors.Include(actor => actor.Movies).SingleOrDefault(a =>
            a.FirstName.ToLower() == actor.FirstName.ToLower() && a.LastName.ToLower() == actor.LastName.ToLower());

        Movie movie = new Movie()
            { Name = "A New Movie To Test Add Actor", Price = 15, ReleaseYear = 2000, GenreId = 1, DirectorId = 1 };
        _dbContext.Movies.Add(movie);
        _dbContext.SaveChanges();
        Movie addedMovie = _dbContext.Movies.SingleOrDefault(m => m.Name.ToLower() == movie.Name.ToLower());

        // Arrange
        AddActorCommand command = new AddActorCommand(_dbContext);
        command.Id = addedMovie.Id;
        command.Model = new AddActorModel() { ActorId = addedActor.Id };

        // Act
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // Assert
        Movie movieToTest = _dbContext.Movies.Include(movie => movie.Actors)
            .SingleOrDefault(movie => movie.Id == command.Id);
        bool hasNewActor = movie.Actors.Any(actor => actor.Id == command.Model.ActorId);
        hasNewActor.Should().BeTrue();
    }
}