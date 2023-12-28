using FluentAssertions;
using MovieStore.Application.ActorOperations.Commands.DeleteActor;
using MovieStore.DBOperations;
using MovieStore.Entities;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.ActorOperations.Commands.DeleteActor;

public class DeleteActorCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly MovieStoreDbContext _context;

    public DeleteActorCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
    }

    [Fact]
    public void Handle_ThrowsInvalidOperationException_WhenGivenActorIsNotFound()
    {
        // Arrange
        DeleteActorCommand command = new(_context);
        command.Id = 999;

        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("Actor not found.");
    }

    [Fact]
    public void Handle_ThrowsInvalidOperationException_WhenGivenActorIsCurrentlyPlayingInAMovie()
    {
        // Arrange
        DeleteActorCommand command = new(_context);
        command.Id = 2;

        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("This actor is currently playing in a movie and cannot be deleted.");
    }

    [Fact]
    public void Handle_DeletesActor_WhenValidInputsAreGiven()
    {
        // Arrange
        Actor actorWithNoMovies = new()
        {
            FirstName = "Actor with",
            LastName = "No movie",
        };

        _context.Actors.Add(actorWithNoMovies);
        _context.SaveChanges();

        Actor createdActor = _context.Actors.SingleOrDefault(a =>
            ((a.FirstName.ToLower() + " " + a.LastName.ToLower()) == (actorWithNoMovies.FirstName.ToLower() +
                                                                      " " + actorWithNoMovies.LastName.ToLower())));

        DeleteActorCommand command = new(_context);
        command.Id = createdActor.Id;

        // Act
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // Assert
        Actor deletedActor = _context.Actors.SingleOrDefault(a => a.Id == command.Id);

        deletedActor.Should().BeNull();
    }
}