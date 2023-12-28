using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using MovieStore.Application.CustomerOperations.Commands.CreateCustomer;
using MovieStore.Application.CustomerOperations.Commands.CreateToken;
using MovieStore.Application.CustomerOperations.Commands.RefreshToken;
using MovieStore.DBOperations;
using MovieStore.Entities;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.CustomerOperations.Commands.RefreshToken;

public class RefreshTokenCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly MovieStoreDbContext _dbContext;
    private readonly IConfiguration _configuration;

    private readonly IMapper _mapper;

    public RefreshTokenCommandTests(CommonTestFixture testFixture)
    {
        _dbContext = testFixture.Context;
        _configuration = testFixture.Configuration;
        _mapper = testFixture.Mapper;
    }

    [Fact]
    public void WhenValidInputsAreGiven_AccessToken_ShouldBeCreatedWithRefreshToken()
    {
        // arrange
        CreateCustomerModel customerModel = new CreateCustomerModel()
        {
            Email = "customertotestrefreshtoken@example.com",
            Password = "customertotestrefreshtoken123",
            FirstName = "customertotestrefreshtoken",
            LastName = "customerlntotestrefreshtoken",
        };

        Customer customer = _mapper.Map<Customer>(customerModel);
        _dbContext.Customers.Add(customer);
        _dbContext.SaveChanges();

        CreateTokenCommand command = new CreateTokenCommand(_dbContext, _configuration);
        command.Model = new LoginModel()
        {
            Email = customerModel.Email,
            Password = customerModel.Password
        };

        // act & assert

        var token = command.Handle();

        RefreshTokenCommand refreshTokenCommand = new RefreshTokenCommand(_dbContext, _configuration);
        refreshTokenCommand.RefreshToken = token.RefreshToken;

        var newToken = refreshTokenCommand.Handle();

        newToken.Should().NotBeNull();
        newToken.AccessToken.Should().NotBeNull();
        newToken.RefreshToken.Should().NotBeNull();
        newToken.RefreshToken.Should().NotBe(token.RefreshToken);
        newToken.ExpirationDate.Should().BeAfter(token.ExpirationDate);
    }

    [Fact]
    public void WhenInvalidInputsAreGiven_Handle_ThrowsInvalidOperationException()
    {
        RefreshTokenCommand command = new RefreshTokenCommand(_dbContext, _configuration);
        command.RefreshToken = "invalid refresh token";

        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("Geçerli bir Refresh Token bulunamadı.");
    }
}