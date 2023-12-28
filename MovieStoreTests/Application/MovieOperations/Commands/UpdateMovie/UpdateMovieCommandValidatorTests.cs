using FluentAssertions;
using MovieStore.Application.MovieOperations.Commands.UpdateMovie;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.MovieOperations.Commands.UpdateMovie;

public class UpdateMovieCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    [Theory]
    //  directorId - genreId - price - id
    // InlineData[(0, 0, 0, 1)] - Valid
    [InlineData(0, 0, 0, 0)]
    [InlineData(0, 0, -1, 1)]
    [InlineData(0, -1, 0, 1)]
    [InlineData(-1, 0, 0, 1)]
    public void WhenInvalidInputsAreGiven_Validator_ShouldReturnErrors(int directorId, int genreId, int price, int id)
    {
        // arrange
        UpdateMovieCommand command = new UpdateMovieCommand(null);
        command.Id = id;
        command.Model = new UpdateMovieModel()
        {
            Name = "A Valid Movie Name",
            DirectorId = directorId,
            GenreId = genreId,
            Price = price
        };

        // act
        UpdateMovieCommandValidator validator = new UpdateMovieCommandValidator();
        var validationResult = validator.Validate(command);

        // assert
        validationResult.Errors.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void WhenGivenReleaseYearIsGreaterThanCurrentYear_Validator_ShouldReturnError()
    {
        // arrange
        UpdateMovieCommand command = new UpdateMovieCommand(null);
        command.Model = new UpdateMovieModel()
        {
            Name = "Lord of The Rings",
            DirectorId = 1,
            GenreId = 1,
            Price = 20,
            ReleaseYear = DateTime.Now.Date.AddYears(5).Year
        };

        // act
        UpdateMovieCommandValidator validator = new UpdateMovieCommandValidator();
        var validationResult = validator.Validate(command);

        // assert
        validationResult.Errors.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void WhenValidInputsAreGiven_Validator_ShouldNotReturnError()
    {
        // arrange
        UpdateMovieCommand command = new UpdateMovieCommand(null);
        command.Id = 1;
        command.Model = new UpdateMovieModel()
        {
            Name = "A Valid Movie Name",
            Price = 10,
            GenreId = 1,
            DirectorId = 3,
            ReleaseYear = 1997
        };

        // act
        UpdateMovieCommandValidator validator = new UpdateMovieCommandValidator();
        var validationResult = validator.Validate(command);

        // assert
        validationResult.Errors.Count.Should().Be(0);
    }
}