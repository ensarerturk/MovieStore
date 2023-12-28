using FluentAssertions;
using MovieStore.Application.ActorOperations.Commands.DeleteActor;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.ActorOperations.Commands.DeleteActor;

public class DeleteActorCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    [Fact]
    public void Validator_ShouldReturnError_WhenNonPositiveIdIsGiven()
    {
        // Arrange
        DeleteActorCommand command = new DeleteActorCommand(null);
        command.Id = 0;

        // Act
        DeleteActorCommandValidator validator = new DeleteActorCommandValidator();
        var validationResult = validator.Validate(command);

        // Assert
        validationResult.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Validator_ShouldNotReturnError_WhenPositiveIdIsGiven()
    {
        // Arrange
        DeleteActorCommand command = new DeleteActorCommand(null);
        command.Id = 1;

        // Act
        DeleteActorCommandValidator validator = new DeleteActorCommandValidator();
        var validationResult = validator.Validate(command);

        // Assert
        validationResult.Errors.Should().BeEmpty();
    }
}