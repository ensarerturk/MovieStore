using AutoMapper;
using FluentAssertions;
using MovieStore.Application.ActorOperations.Queries.GetActorById;
using MovieStore.DBOperations;
using MovieStore.Entities;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.ActorOperations.Queries.GetActorById;

public class GetActorByIdQueryTests : IClassFixture<CommonTestFixture>
{
    private readonly MovieStoreDbContext _context;
    private readonly IMapper _mapper;

    public GetActorByIdQueryTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

    [Fact]
    public void Handle_ThrowsInvalidOperationException_WhenGivenActorIsNotFound()
    {
        // Arrange
        GetActorByIdQuery query = new(_context, _mapper);
        query.Id = 999;

        // Act & Assert
        FluentActions
            .Invoking(() => query.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("No found actor.");
    }

    [Fact]
    public void Handle_ShouldReturnActor_WhenValidInputsAreGiven()
    {
        // Arrange
        GetActorByIdQuery query = new(_context, _mapper);
        query.Id = 2;

        // Act
        var result = query.Handle();

        // Assert
        Actor? actor = _context.Actors.SingleOrDefault(a => a.Id == query.Id);

        actor.Should().NotBeNull();
        result.Should().NotBeNull();
        result.FirstName.Should().Be(actor.FirstName);
        result.LastName.Should().Be(actor.LastName);
    }
}