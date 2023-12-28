using FluentAssertions;
using MovieStore.Application.DirectorOperations.Queries.GetDirectorById;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.DirectorOperations.Queries.GetDirectorById;

public class GetDirectorByIdQueryValidatorTests : IClassFixture<CommonTestFixture>
{
    [Fact]
    public void WhenNonPositiveIdIsGiven_Validator_ShouldReturnError()
    {
        // arrange
        GetDirectorByIdQuery query = new GetDirectorByIdQuery(null, null);
        query.Id = 0;

        // act
        GetDirectorByIdQueryValidator validator = new GetDirectorByIdQueryValidator();
        var validationResult = validator.Validate(query);

        // // assert
        validationResult.Errors.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void WhenPositiveIdIsGiven_Validator_ShouldNotReturnError()
    {
        // arrange
        GetDirectorByIdQuery query = new GetDirectorByIdQuery(null, null);
        query.Id = 1;

        // act
        GetDirectorByIdQueryValidator validator = new GetDirectorByIdQueryValidator();
        var validationResult = validator.Validate(query);

        // // assert
        validationResult.Errors.Count.Should().Be(0);
    }
}