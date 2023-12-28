using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using MovieStore.Application.CustomerOperations.Commands.CreateCustomer;
using MovieStore.Application.CustomerOperations.Commands.CreateToken;
using MovieStore.DBOperations;
using MovieStore.Entities;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.CustomerOperations.Commands.CreateToken;

public class CreateTokenCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly MovieStoreDbContext _dbContext;
    private readonly IMapper _mapper;

    private readonly IConfiguration _configuration;

    public CreateTokenCommandTests(CommonTestFixture testFixture)
    {
        _dbContext = testFixture.Context;
        _mapper = testFixture.Mapper;
        _configuration = testFixture.Configuration;
    }

    [Theory]
    [InlineData("nonexisting@example.com", "nonexisting")]
    [InlineData("furkan123@example.com", "nonexisting")]
    [InlineData("nonexisting@example.com", "furkan123")]
    public void WhenGivenCredentialsAreInvalid_Handle_ThrowsInvalidOperationException(string email, string password)
    {
        // arrange
        CreateTokenCommand command = new CreateTokenCommand(_dbContext, _configuration);
        command.Model = new LoginModel()
        {
            Email = email,
            Password = password,
        };

        // act & assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("Invalid username or password.");
    }

    [Fact]
    public void WhenValidInputsAreGiven_Customer_ShouldBeCreated()
    {
        // arrange
        var customerModel = new CreateCustomerModel()
        {
            Email = "newcustomertotesttoken@example.com",
            Password = "newcustomertotesttoken",
            FirstName = "newcustomertotesttoken",
            LastName = "newcustomerlntotesttoken",
        };

        var customer = _mapper.Map<Customer>(customerModel);
        _dbContext.Customers.Add(customer);
        _dbContext.SaveChanges();

        CreateTokenCommand command = new CreateTokenCommand(_dbContext, _configuration);
        command.Model = new LoginModel()
        {
            Email = customerModel.Email,
            Password = customerModel.Password,
        };

        // act & assert

        var token = command.Handle();
        token.Should().NotBeNull();
        token.AccessToken.Should().NotBeNull();
        token.RefreshToken.Should().NotBeNull();
        token.ExpirationDate.Should().BeAfter(DateTime.Now);
    }
}