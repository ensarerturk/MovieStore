using FluentAssertions;
using MovieStore.Application.DirectorOperations.Commands.DeleteDirector;
using MovieStore.DBOperations;
using MovieStore.Entities;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.DirectorOperations.Commands.DeleteDirector;

public class DeleteDirectorCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly MovieStoreDbContext _context;

    public DeleteDirectorCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
    }

    [Fact]
    public void WhenGivenDirectorIsNotFound_Handle_ThrowsInvalidOperationException()
    {
        // arrange
        DeleteDirectorCommand command = new DeleteDirectorCommand(_context);
        command.Id = 999;

        // act & assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("No found director.");
    }

    [Fact]
    public void WhenGivenDirectorIsDirectingAMovie_Handle_ThrowsInvalidOperationException()
    {
        // arrange
        DeleteDirectorCommand command = new DeleteDirectorCommand(_context);
        command.Id = 2;

        // act & assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("This director is directing a movie, which cannot be deleted right now.");
    }

    [Fact]
    public void WhenValidInputsAreGiven_Director_ShouldBeDeleted()
    {
        // arrange
        Director actorWithNoMovies = new Director()
        {
            FirstName = "Director with",
            LastName = "No movie",
        };

        _context.Directors.Add(actorWithNoMovies);
        _context.SaveChanges();

        Director createdDirector = _context.Directors.SingleOrDefault(author =>
            ((author.FirstName.ToLower() + " " + author.LastName.ToLower()) == (actorWithNoMovies.FirstName.ToLower() +
                " " + actorWithNoMovies.LastName.ToLower())));

        DeleteDirectorCommand command = new DeleteDirectorCommand(_context);
        command.Id = createdDirector.Id;

        // act
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // assert
        Director author = _context.Directors.SingleOrDefault(author => author.Id == command.Id);

        author.Should().BeNull();
    }
}