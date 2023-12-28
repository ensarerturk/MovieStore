using FluentAssertions;
using MovieStore.Application.MovieOperations.Commands.CreateMovie;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.MovieOperations.Commands.CreateMovie;

public class CreateMovieCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    [Theory]
    [InlineData("Lord Of The Rings", 1, 1, 0)]
    [InlineData("Lord Of The Rings", 1, 0, 1)]
    [InlineData("Lord Of The Rings", 0, 1, 1)]
    [InlineData("", 1, 1, 1)]
    [InlineData(" ", 1, 1, 1)]
    [InlineData("     ", 1, 1, 1)]
    public void WhenInvalidInputsAreGiven_Validator_ShouldReturnErrors(string name, int directorId, int genreId,
        int price)
    {
        // arrange
        CreateMovieCommand command = new CreateMovieCommand(null, null);
        command.Model = new CreateMovieModel()
        {
            Name = name,
            DirectorId = directorId,
            GenreId = genreId,
            Price = price,
            ReleaseYear = DateTime.Now.Date.AddYears(-1).Year,
        };

        // act
        CreateMovieCommandValidator validator = new CreateMovieCommandValidator();
        var validationResult = validator.Validate(command);

        // assert
        validationResult.Errors.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void WhenGivenReleaseYearIsGreaterThanCurrentYear_Validator_ShouldReturnError()
    {
        // arrange
        CreateMovieCommand command = new CreateMovieCommand(null, null);
        command.Model = new CreateMovieModel()
        {
            Name = "Lord of The Rings",
            DirectorId = 1,
            GenreId = 1,
            Price = 20,
            ReleaseYear = DateTime.Now.Date.AddYears(5).Year
        };

        // act
        CreateMovieCommandValidator validator = new CreateMovieCommandValidator();
        var validationResult = validator.Validate(command);

        // assert
        validationResult.Errors.Count.Should().BeGreaterThan(0);
    }


    [Fact]
    public void WhenValidInputsAreGiven_Validator_ShouldNotReturnError()
    {
        // arrange
        CreateMovieCommand command = new CreateMovieCommand(null, null);
        command.Model = new CreateMovieModel()
        {
            Name = "Lord of The Rings",
            DirectorId = 1,
            GenreId = 1,
            Price = 20,
            ReleaseYear = DateTime.Now.Date.AddYears(-5).Year
        };

        // act
        CreateMovieCommandValidator validator = new CreateMovieCommandValidator();
        var validationResult = validator.Validate(command);

        // assert
        validationResult.Errors.Count.Should().Be(0);
    }
}