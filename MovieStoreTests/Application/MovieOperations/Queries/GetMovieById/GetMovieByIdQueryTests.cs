using AutoMapper;
using FluentAssertions;
using MovieStore.Application.MovieOperations.Queries.GetMovieById;
using MovieStore.DBOperations;
using MovieStore.Entities;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.MovieOperations.Queries.GetMovieById;

public class GetMovieByIdQueryTests : IClassFixture<CommonTestFixture>
{
    private readonly MovieStoreDbContext _context;
    private readonly IMapper _mapper;

    public GetMovieByIdQueryTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

    [Fact]
    public void WhenGivenMovieIsNotFound_Handle_ThrowsInvalidOperationException()
    {
        // arrange
        GetMovieByIdQuery query = new GetMovieByIdQuery(_context, _mapper);
        query.Id = 999;

        // act & assert
        FluentActions
            .Invoking(() => query.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("Film bulunamadı.");
    }

    [Fact]
    public void WhenValidInputsAreGiven_Movie_ShouldBeReturned()
    {
        // arrange
        GetMovieByIdQuery query = new GetMovieByIdQuery(_context, _mapper);
        query.Id = 2;

        // act
        FluentActions.Invoking(() => query.Handle()).Invoke();

        // assert
        Movie movie = _context.Movies.SingleOrDefault(movie => movie.Id == query.Id);

        movie.Should().NotBeNull();
    }
}