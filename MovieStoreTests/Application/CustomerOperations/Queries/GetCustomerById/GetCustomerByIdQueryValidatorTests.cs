using FluentAssertions;
using MovieStore.Application.CustomerOperations.Queries.GetCustomerById;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.CustomerOperations.Queries.GetCustomerById;

public class GetCustomerByIdQueryValidatorTests : IClassFixture<CommonTestFixture>
{
    [Fact]
    public void WhenNonPositiveIdIsGiven_Validator_ShouldReturnError()
    {
        // arrange
        GetCustomerByIdQuery query = new GetCustomerByIdQuery(null, null);
        query.Id = 0;

        // act
        GetCustomerByIdQueryValidator validator = new GetCustomerByIdQueryValidator();
        var validationResult = validator.Validate(query);

        // // assert
        validationResult.Errors.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void WhenPositiveIdIsGiven_Validator_ShouldNotReturnError()
    {
        // arrange
        GetCustomerByIdQuery query = new GetCustomerByIdQuery(null, null);
        query.Id = 1;

        // act
        GetCustomerByIdQueryValidator validator = new GetCustomerByIdQueryValidator();
        var validationResult = validator.Validate(query);

        // // assert
        validationResult.Errors.Count.Should().Be(0);
    }
}