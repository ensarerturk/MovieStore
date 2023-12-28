using FluentAssertions;
using MovieStore.Application.ActorOperations.Commands.CreateActor;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.ActorOperations.Commands.CreateActor;

public class CreateActorCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    [Theory]
    // firstName - lastName
    [InlineData("New", "Actor")] // Valid case
    [InlineData("New", "")]
    [InlineData("New", " ")]
    [InlineData("New", "  ")]
    [InlineData("New", "   ")]
    [InlineData("", "Actor")]
    [InlineData(" ", "Actor")]
    [InlineData("   ", "Actor")]
    [InlineData("", "")]
    [InlineData(" ", " ")]
    [InlineData("    ", "    ")]
    public void Validator_ShouldReturnErrors_WhenInvalidInputsAreGiven(string firstName, string lastName)
    {
        // Arrange
        CreateActorCommand command = new CreateActorCommand(null, null);
        command.Model = new CreateActorModel()
        {
            FirstName = firstName,
            LastName = lastName,
        };

        // Act
        CreateActorCommandValidator validator = new CreateActorCommandValidator();
        var validationResult = validator.Validate(command);

        // Assert
        validationResult.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Validator_ShouldNotReturnError_WhenValidInputsAreGiven()
    {
        // Arrange
        CreateActorCommand command = new CreateActorCommand(null, null);
        command.Model = new CreateActorModel()
        {
            FirstName = "New",
            LastName = "Actor",
        };

        // Act
        CreateActorCommandValidator validator = new CreateActorCommandValidator();
        var validationResult = validator.Validate(command);

        // Assert
        validationResult.Errors.Should().BeEmpty();
    }
}