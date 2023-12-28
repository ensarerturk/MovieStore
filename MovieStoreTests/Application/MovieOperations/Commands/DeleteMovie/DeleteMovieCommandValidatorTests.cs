using FluentAssertions;
using MovieStore.Application.MovieOperations.Commands.DeleteMovie;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.MovieOperations.Commands.DeleteMovie;

public class DeleteMovieCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    [Fact]
    public void WhenNonPositiveIdIsGiven_Validator_ShouldReturnError()
    {
        // arrange
        DeleteMovieCommand command = new DeleteMovieCommand(null);
        command.Id = 0;

        // act
        DeleteMovieCommandValidator validator = new DeleteMovieCommandValidator();
        var validationResult = validator.Validate(command);

        // // assert
        validationResult.Errors.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void WhenPositiveIdIsGiven_Validator_ShouldNotReturnError()
    {
        // arrange
        DeleteMovieCommand command = new DeleteMovieCommand(null);
        command.Id = 1;

        // act
        DeleteMovieCommandValidator validator = new DeleteMovieCommandValidator();
        var validationResult = validator.Validate(command);

        // assert
        validationResult.Errors.Count.Should().Be(0);
    }
}