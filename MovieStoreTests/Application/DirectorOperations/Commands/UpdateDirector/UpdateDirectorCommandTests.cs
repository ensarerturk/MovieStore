using FluentAssertions;
using MovieStore.Application.DirectorOperations.Commands.UpdateDirector;
using MovieStore.DBOperations;
using MovieStore.Entities;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.DirectorOperations.Commands.UpdateDirector;

public class UpdateDirectorCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly MovieStoreDbContext _context;

    public UpdateDirectorCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
    }

    [Fact]
    public void WhenGivenDirectorIsNotFound_Handle_ThrowsInvalidOperationException()
    {
        // Arrange
        UpdateDirectorCommand command = new UpdateDirectorCommand(_context);
        command.Id = 999;
        command.Model = new UpdateDirectorModel() { FirstName = "Updated", LastName = "Director" };

        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("No found director.");
    }

    [Fact]
    public void WhenGivenDirectorNameAlreadyExistsWithDifferentId_Handle_ThrowsInvalidOperationException()
    {
        // Arrange
        UpdateDirectorCommand command = new UpdateDirectorCommand(_context);
        command.Id = 3;
        command.Model = new UpdateDirectorModel() { FirstName = "Quentin", LastName = "Tarantino" };

        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("There is already a director with this name.");
    }

    [Fact]
    public void WhenDefaultInputsAreGiven_Director_ShouldNotBeChanged()
    {
        // arrange
        Director newDirector = new Director() { FirstName = "A new", LastName = "Director name" };
        _context.Directors.Add(newDirector);
        _context.SaveChanges();
        Director addedDirector = _context.Directors.SingleOrDefault(director =>
            director.FirstName.ToLower() == newDirector.FirstName.ToLower() &&
            director.LastName.ToLower() == newDirector.LastName.ToLower());

        UpdateDirectorCommand command = new UpdateDirectorCommand(_context);
        command.Id = addedDirector.Id;
        UpdateDirectorModel model = new UpdateDirectorModel()
        {
            FirstName = "",
            LastName = ""
        };
        command.Model = model;

        Director directorBeforeUpdate = _context.Directors.SingleOrDefault(director => director.Id == command.Id);

        // act
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // assert
        Director directorAfterUpdate = _context.Directors.SingleOrDefault(director => director.Id == command.Id);

        directorAfterUpdate.Should().NotBeNull();
        directorAfterUpdate.FirstName.Should().Be(directorBeforeUpdate.FirstName);
        directorAfterUpdate.LastName.Should().Be(directorBeforeUpdate.LastName);
    }

    [Fact]
    public void WhenValidInputsAreGiven_Director_ShouldBeUpdated()
    {
        // arrange
        UpdateDirectorCommand command = new UpdateDirectorCommand(_context);
        command.Id = 3;
        UpdateDirectorModel model = new UpdateDirectorModel()
        {
            FirstName = "Updated",
            LastName = "Director Name",
        };
        command.Model = model;

        // act
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // assert
        Director director = _context.Directors.SingleOrDefault(director => director.Id == command.Id);

        director.Should().NotBeNull();
        director.FirstName.Should().Be(model.FirstName);
        director.LastName.Should().Be(model.LastName);
    }
}