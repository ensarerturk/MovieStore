using AutoMapper;
using FluentAssertions;
using MovieStore.Application.DirectorOperations.Queries.GetDirectorById;
using MovieStore.DBOperations;
using MovieStore.Entities;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.DirectorOperations.Queries.GetDirectorById;

public class GetDirectorByIdQueryTests : IClassFixture<CommonTestFixture>
{
    private readonly MovieStoreDbContext _context;
    private readonly IMapper _mapper;

    public GetDirectorByIdQueryTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

    [Fact]
    public void WhenGivenDirectorIsNotFound_Handle_ThrowsInvalidOperationException()
    {
        // arrange
        GetDirectorByIdQuery query = new GetDirectorByIdQuery(_context, _mapper);
        query.Id = 999;

        // act & assert
        FluentActions
            .Invoking(() => query.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("No found director.");
    }

    [Fact]
    public void WhenValidInputsAreGiven_Director_ShouldBeReturned()
    {
        // arrange
        GetDirectorByIdQuery query = new GetDirectorByIdQuery(_context, _mapper);
        query.Id = 2;

        // act
        FluentActions.Invoking(() => query.Handle()).Invoke();

        // assert
        Director author = _context.Directors.SingleOrDefault(author => author.Id == query.Id);

        author.Should().NotBeNull();
    }
}