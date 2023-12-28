using AutoMapper;
using FluentAssertions;
using MovieStore.Application.ActorOperations.Commands.CreateActor;
using MovieStore.DBOperations;
using MovieStore.Entities;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.ActorOperations.Commands.CreateActor;

public class CreateActorCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly MovieStoreDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateActorCommandTests(CommonTestFixture testFixture)
    {
        _dbContext = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

    [Fact]
    public void Handle_ThrowsInvalidOperationException_WhenDuplicateActorNameIsGiven()
    {
        // Arrange
        Actor existingActor = new Actor()
        {
            FirstName = "Existing",
            LastName = "Actor"
        };
        _dbContext.Actors.Add(existingActor);
        _dbContext.SaveChanges();

        CreateActorCommand command = new CreateActorCommand(_dbContext, _mapper);
        command.Model = new CreateActorModel()
        {
            FirstName = existingActor.FirstName,
            LastName = existingActor.LastName
        };

        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("Actor already exists.");
    }

    [Fact]
    public void Handle_CreatesActor_WhenValidInputsAreGiven()
    {
        // Arrange
        CreateActorCommand command = new CreateActorCommand(_dbContext, _mapper);
        var model = new CreateActorModel()
        {
            FirstName = "New",
            LastName = "Actor"
        };
        command.Model = model;

        // Act
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // Assert
        var actor = _dbContext.Actors.SingleOrDefault(a =>
            a.FirstName.ToLower() == model.FirstName.ToLower() &&
            a.LastName.ToLower() == model.LastName.ToLower());

        actor.Should().NotBeNull();
        actor.FirstName.Should().Be(model.FirstName);
        actor.LastName.Should().Be(model.LastName);
    }
}