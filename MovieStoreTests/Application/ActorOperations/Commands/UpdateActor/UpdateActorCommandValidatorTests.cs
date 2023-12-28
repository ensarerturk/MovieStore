using FluentAssertions;
using MovieStore.Application.ActorOperations.Commands.UpdateActor;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.ActorOperations.Commands.UpdateActor;

public class UpdateActorCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    [Fact]
    public void WhenValidInputsAreGiven_Validator_ShouldNotReturnError()
    {
        // arrange
        UpdateActorCommand command = new UpdateActorCommand(null);
        command.Id = 1;
        command.Model = new UpdateActorModel()
        {
            FirstName = "Updated",
            LastName = "Actor Name"
        };

        // act
        UpdateActorCommandValidator validator = new UpdateActorCommandValidator();
        var validationResult = validator.Validate(command);

        // assert
        validationResult.Errors.Count.Should().Be(0);
    }
}