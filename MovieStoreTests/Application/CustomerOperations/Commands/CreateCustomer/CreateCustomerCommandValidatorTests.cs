using FluentAssertions;
using MovieStore.Application.CustomerOperations.Commands.CreateCustomer;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.CustomerOperations.Commands.CreateCustomer;

public class CreateCustomerCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    [Theory]
    // email - password - firstName - lastName
    // [InlineData("newcustomer@example.com", "newcustomer", "firstname", "lastname")] - Geçerli
    [InlineData("", "newcustomer", "firstname", "lastname")]
    [InlineData(" ", "newcustomer", "firstname", "lastname")]
    [InlineData("    ", "newcustomer", "firstname", "lastname")]
    [InlineData("  aaa   ", "newcustomer", "firstname", "lastname")]
    [InlineData("in%va%lid%email.com", "newcustomer", "firstname", "lastname")]
    [InlineData("example.com", "newcustomer", "firstname", "lastname")]
    [InlineData("A@b@c@domain.com", "newcustomer", "firstname", "lastname")]
    [InlineData("newcustomer@example.com", "", "firstname", "lastname")]
    [InlineData("newcustomer@example.com", " ", "firstname", "lastname")]
    [InlineData("newcustomer@example.com", "   ", "firstname", "lastname")]
    [InlineData("newcustomer@example.com", "a", "firstname", "lastname")]
    [InlineData("newcustomer@example.com", "ab", "firstname", "lastname")]
    [InlineData("newcustomer@example.com", "abc", "firstname", "lastname")]
    [InlineData("newcustomer@example.com", "abcd", "firstname", "lastname")]
    [InlineData("newcustomer@example.com", "abcde", "firstname", "lastname")]
    [InlineData("newcustomer@example.com", "newcustomer", "", "lastname")]
    [InlineData("newcustomer@example.com", "newcustomer", " ", "lastname")]
    [InlineData("newcustomer@example.com", "newcustomer", "firstname", "")]
    [InlineData("newcustomer@example.com", "newcustomer", "firstname", " ")]
    public void Validate_ShouldReturnErrors_WhenInvalidInputsAreGiven(string email, string password, string firstName,
        string lastName)
    {
        // Arrange
        CreateCustomerCommand command = new(null, null);
        command.Model = new CreateCustomerModel()
        {
            Email = email,
            Password = password,
            FirstName = firstName,
            LastName = lastName
        };

        // Act
        CreateCustomerCommandValidator validator = new();
        var validationResult = validator.Validate(command);

        // Assert
        validationResult.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Validate_ShouldNotReturnError_WhenValidInputsAreGiven()
    {
        // Arrange
        CreateCustomerCommand command = new(null, null);
        command.Model = new CreateCustomerModel()
        {
            Email = "newcustomer@example.com",
            Password = "newcustomer",
            FirstName = "firstname",
            LastName = "lastname"
        };

        // Act
        CreateCustomerCommandValidator validator = new();
        var validationResult = validator.Validate(command);

        // Assert
        validationResult.Errors.Should().BeEmpty();
    }
}