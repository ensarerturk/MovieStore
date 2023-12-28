using FluentAssertions;
using MovieStore.Application.MovieOperations.Commands.BuyMovie;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.MovieOperations.Commands.BuyMovie;

public class BuyMovieCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    [Fact]
    public void WhenNonPositiveIdIsGiven_Validator_ShouldReturnError()
    {
        // arrange
        BuyMovieCommand command = new BuyMovieCommand(null, null);
        command.Id = 0;

        // act
        BuyMovieCommandValidator validator = new BuyMovieCommandValidator();
        var validationResult = validator.Validate(command);

        // // assert
        validationResult.Errors.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void WhenPositiveIdIsGiven_Validator_ShouldReturnError()
    {
        // arrange
        BuyMovieCommand command = new BuyMovieCommand(null, null);
        command.Id = 1;

        // act
        BuyMovieCommandValidator validator = new BuyMovieCommandValidator();
        var validationResult = validator.Validate(command);

        // // assert
        validationResult.Errors.Count.Should().Be(0);
    }
}