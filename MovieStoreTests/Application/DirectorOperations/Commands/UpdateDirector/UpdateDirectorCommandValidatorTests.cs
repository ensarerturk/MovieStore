using FluentAssertions;
using MovieStore.Application.DirectorOperations.Commands.UpdateDirector;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.DirectorOperations.Commands.UpdateDirector;

public class UpdateDirectorCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    [Fact]
    public void WhenValidInputsAreGiven_Validator_ShouldNotReturnError()
    {
        // arrange
        UpdateDirectorCommand command = new UpdateDirectorCommand(null);
        command.Id = 1;
        command.Model = new UpdateDirectorModel()
        {
            FirstName = "Updated",
            LastName = "Director Name"
        };

        // act
        UpdateDirectorCommandValidator validator = new UpdateDirectorCommandValidator();
        var validationResult = validator.Validate(command);

        // assert
        validationResult.Errors.Count.Should().Be(0);
    }
}