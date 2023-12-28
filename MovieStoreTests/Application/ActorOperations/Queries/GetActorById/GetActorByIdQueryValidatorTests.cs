using FluentAssertions;
using MovieStore.Application.ActorOperations.Queries.GetActorById;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.ActorOperations.Queries.GetActorById;

public class GetActorByIdQueryValidatorTests : IClassFixture<CommonTestFixture>
{
    [Fact]
    public void Validator_ShouldReturnError_WhenNonPositiveIdIsGiven()
    {
        // Arrange
        GetActorByIdQuery query = new(null, null);
        query.Id = 0;

        // Act
        GetActorByIdQueryValidator validator = new();
        var validationResult = validator.Validate(query);

        // Assert
        validationResult.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Validator_ShouldNotReturnError_WhenPositiveIdIsGiven()
    {
        // Arrange
        GetActorByIdQuery query = new(null, null);
        query.Id = 1;

        // Act
        GetActorByIdQueryValidator validator = new();
        var validationResult = validator.Validate(query);

        // Assert
        validationResult.Errors.Should().BeEmpty();
    }
}