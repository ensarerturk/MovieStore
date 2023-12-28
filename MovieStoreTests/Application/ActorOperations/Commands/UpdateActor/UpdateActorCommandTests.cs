using FluentAssertions;
using MovieStore.Application.ActorOperations.Commands.UpdateActor;
using MovieStore.DBOperations;
using MovieStore.Entities;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.ActorOperations.Commands.UpdateActor;

public class UpdateActorCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly MovieStoreDbContext _context;

    public UpdateActorCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
    }

    [Fact]
    public void Handle_ThrowsInvalidOperationException_WhenGivenActorIsNotFound()
    {
        // Arrange
        UpdateActorCommand command = new(_context);
        command.Id = 999;
        command.Model = new UpdateActorModel() { FirstName = "Updated", LastName = "Actor" };

        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("No found actor.");
    }

    [Fact]
    public void Handle_ThrowsInvalidOperationException_WhenGivenActorNameAlreadyExistsWithDifferentId()
    {
        // Arrange
        UpdateActorCommand command = new(_context);
        command.Id = 3;
        command.Model = new UpdateActorModel() { FirstName = "Alison", LastName = "Brie" };

        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("This actor already exists.");
    }

    [Fact]
    public void Handle_ShouldNotChangeActor_WhenDefaultInputsAreGiven()
    {
        // Arrange
        UpdateActorCommand command = new(_context);
        command.Id = 3;
        UpdateActorModel model = new()
        {
            FirstName = "",
            LastName = ""
        };
        command.Model = model;

        Actor actorBeforeUpdate = _context.Actors.SingleOrDefault(a => a.Id == command.Id);

        // Act
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // Assert
        Actor actorAfterUpdate = _context.Actors.SingleOrDefault(a => a.Id == command.Id);

        actorAfterUpdate.Should().NotBeNull();
        actorAfterUpdate.FirstName.Should().Be(actorBeforeUpdate.FirstName);
        actorAfterUpdate.LastName.Should().Be(actorBeforeUpdate.LastName);
    }

    [Fact]
    public void Handle_ShouldUpdateActor_WhenValidInputsAreGiven()
    {
        // Arrange
        UpdateActorCommand command = new(_context);
        command.Id = 3;
        UpdateActorModel model = new()
        {
            FirstName = "Updated",
            LastName = "Actor Name",
        };
        command.Model = model;

        // Act
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // Assert
        Actor actor = _context.Actors.SingleOrDefault(a => a.Id == command.Id);

        actor.Should().NotBeNull();
        actor.FirstName.Should().Be(model.FirstName);
        actor.LastName.Should().Be(model.LastName);
    }
}