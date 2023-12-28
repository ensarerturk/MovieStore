using FluentAssertions;
using MovieStore.Application.DirectorOperations.Commands.DeleteDirector;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.DirectorOperations.Commands.DeleteDirector;

public class DeleteDirectorCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    [Fact]
    public void WhenNonPositiveIdIsGiven_Validator_ShouldReturnError()
    {
        // arrange
        DeleteDirectorCommand command = new DeleteDirectorCommand(null);
        command.Id = 0;

        // act
        DeleteDirectorCommandValidator validator = new DeleteDirectorCommandValidator();
        var validationResult = validator.Validate(command);

        // // assert
        validationResult.Errors.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void WhenPositiveIdIsGiven_Validator_ShouldNotReturnError()
    {
        // arrange
        DeleteDirectorCommand command = new DeleteDirectorCommand(null);
        command.Id = 1;

        // act
        DeleteDirectorCommandValidator validator = new DeleteDirectorCommandValidator();
        var validationResult = validator.Validate(command);

        // assert
        validationResult.Errors.Count.Should().Be(0);
    }
}