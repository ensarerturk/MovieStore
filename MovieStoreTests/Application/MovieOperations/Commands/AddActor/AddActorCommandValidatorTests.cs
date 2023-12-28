using FluentAssertions;
using MovieStore.Application.MovieOperations.Commands.AddActor;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.MovieOperations.Commands.AddActor;

public class AddActorCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    public void WhenInvalidInputIsGiven_Validator_ShouldReturnError(int movieId, int actorId)
    {
        // arrange
        AddActorCommand command = new AddActorCommand(null);
        command.Id = movieId;
        command.Model = new AddActorModel() { ActorId = actorId };

        // act
        AddActorCommandValidator validator = new AddActorCommandValidator();
        var validationResult = validator.Validate(command);

        // // assert
        validationResult.Errors.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void WhenValidInputIsGiven_Validator_ShouldReturnError()
    {
        // arrange
        AddActorCommand command = new AddActorCommand(null);
        command.Id = 1;
        command.Model = new AddActorModel() { ActorId = 1 };

        // act
        AddActorCommandValidator validator = new AddActorCommandValidator();
        var validationResult = validator.Validate(command);

        // // assert
        validationResult.Errors.Count.Should().Be(0);
    }
}