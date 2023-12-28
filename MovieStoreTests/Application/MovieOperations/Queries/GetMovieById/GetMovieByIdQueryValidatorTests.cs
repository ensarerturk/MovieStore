using FluentAssertions;
using MovieStore.Application.MovieOperations.Queries.GetMovieById;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.MovieOperations.Queries.GetMovieById;

public class GetMovieByIdQueryValidatorTests : IClassFixture<CommonTestFixture>
{
    [Fact]
    public void WhenNonPositiveIdIsGiven_Validator_ShouldReturnError()
    {
        // arrange
        GetMovieByIdQuery query = new GetMovieByIdQuery(null, null);
        query.Id = 0;

        // act
        GetMovieByIdQueryValidator validator = new GetMovieByIdQueryValidator();
        var validationResult = validator.Validate(query);

        // // assert
        validationResult.Errors.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void WhenPositiveIdIsGiven_Validator_ShouldNotReturnError()
    {
        // arrange
        GetMovieByIdQuery query = new GetMovieByIdQuery(null, null);
        query.Id = 1;

        // act
        GetMovieByIdQueryValidator validator = new GetMovieByIdQueryValidator();
        var validationResult = validator.Validate(query);

        // // assert
        validationResult.Errors.Count.Should().Be(0);
    }
}