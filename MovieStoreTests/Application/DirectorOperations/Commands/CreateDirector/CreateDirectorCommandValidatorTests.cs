using FluentAssertions;
using MovieStore.Application.DirectorOperations.Commands.CreateDirector;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.DirectorOperations.Commands.CreateDirector;

public class CreateDirectorCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    [Theory]
    // firstName - lastName
    // [InlineData("New","Director")] - Valid
    [InlineData("New", "")]
    [InlineData("New", " ")]
    [InlineData("New", "  ")]
    [InlineData("New", "   ")]
    [InlineData("", "Director")]
    [InlineData(" ", "Director")]
    [InlineData("   ", "Director")]
    [InlineData("", "")]
    [InlineData(" ", " ")]
    [InlineData("    ", "    ")]
    public void WhenInvalidInputsAreGiven_Validator_ShouldReturnErrors(string firstName, string lastName)
    {
        // arrange
        CreateDirectorCommand command = new CreateDirectorCommand(null, null);
        command.Model = new CreateDirectorModel()
        {
            FirstName = firstName,
            LastName = lastName,
        };

        // act
        CreateDirectorCommandValidator validator = new CreateDirectorCommandValidator();
        var validationResult = validator.Validate(command);

        // assert
        validationResult.Errors.Count.Should().BeGreaterThan(0);
    }


    [Fact]
    public void WhenValidInputsAreGiven_Validator_ShouldNotReturnError()
    {
        // arrange
        CreateDirectorCommand command = new CreateDirectorCommand(null, null);
        command.Model = new CreateDirectorModel()
        {
            FirstName = "New",
            LastName = "Director",
        };

        // act
        CreateDirectorCommandValidator validator = new CreateDirectorCommandValidator();
        var validationResult = validator.Validate(command);

        // assert
        validationResult.Errors.Count.Should().Be(0);
    }
}