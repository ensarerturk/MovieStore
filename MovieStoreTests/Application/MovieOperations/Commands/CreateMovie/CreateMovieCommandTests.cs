using AutoMapper;
using FluentAssertions;
using MovieStore.Application.MovieOperations.Commands.CreateMovie;
using MovieStore.DBOperations;
using MovieStore.Entities;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.MovieOperations.Commands.CreateMovie;

public class CreateMovieCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly MovieStoreDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateMovieCommandTests(CommonTestFixture testFixture)
    {
        _dbContext = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

    [Fact]
    public void WhenAlreadyExistingMovieNameIsGiven_Handle_ThrowsInvalidOperationException()
    {
        Movie movie = new Movie()
        {
            Name = "Existing Movie",
        };
        _dbContext.Movies.Add(movie);
        _dbContext.SaveChanges();

        // arrange
        CreateMovieCommand command = new CreateMovieCommand(_dbContext, _mapper);
        command.Model = new CreateMovieModel()
        {
            Name = movie.Name,
        };

        // act & assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("The movie already exist.");
    }

    [Fact]
    public void WhenValidInputsAreGiven_Movie_ShouldBeCreated()
    {
        // arrange
        CreateMovieCommand command = new CreateMovieCommand(_dbContext, _mapper);
        var model = new CreateMovieModel()
        {
            Name = "New Movie",
        };
        command.Model = model;

        // act
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // assert
        var movie = _dbContext.Movies.SingleOrDefault(movie => movie.Name.ToLower() == model.Name.ToLower());

        movie.Should().NotBeNull();
        movie.Name.Should().Be(model.Name);
    }
}