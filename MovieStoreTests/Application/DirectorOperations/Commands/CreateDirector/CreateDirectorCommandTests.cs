using AutoMapper;
using FluentAssertions;
using MovieStore.Application.DirectorOperations.Commands.CreateDirector;
using MovieStore.DBOperations;
using MovieStore.Entities;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.DirectorOperations.Commands.CreateDirector;

public class CreateDirectorCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly MovieStoreDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateDirectorCommandTests(CommonTestFixture testFixture)
    {
        _dbContext = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

    [Fact]
    public void WhenAlreadyExistingDirectorNameIsGiven_Handle_ThrowsInvalidOperationException()
    {
        Director director = new Director()
        {
            FirstName = "Existing",
            LastName = "Director"
        };
        _dbContext.Directors.Add(director);
        _dbContext.SaveChanges();

        // arrange
        CreateDirectorCommand command = new CreateDirectorCommand(_dbContext, _mapper);
        command.Model = new CreateDirectorModel()
        {
            FirstName = director.FirstName,
            LastName = director.LastName
        };

        // act & assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("Yönetmen zaten mevcut.");
    }

    [Fact]
    public void WhenValidInputsAreGiven_Director_ShouldBeCreated()
    {
        // arrange
        CreateDirectorCommand command = new CreateDirectorCommand(_dbContext, _mapper);
        var model = new CreateDirectorModel()
        {
            FirstName = "New",
            LastName = "Director"
        };
        command.Model = model;

        // act
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // assert
        var actor = _dbContext.Directors.SingleOrDefault(actor =>
            actor.FirstName.ToLower() == model.FirstName.ToLower() &&
            actor.LastName.ToLower() == model.LastName.ToLower());

        actor.Should().NotBeNull();
        actor.FirstName.Should().Be(model.FirstName);
        actor.LastName.Should().Be(model.LastName);
    }
}