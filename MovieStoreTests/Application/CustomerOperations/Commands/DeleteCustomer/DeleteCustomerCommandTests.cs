using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using MovieStore.Application.CustomerOperations.Commands.DeleteCustomer;
using MovieStore.DBOperations;
using MovieStore.Entities;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.CustomerOperations.Commands.DeleteCustomer;

public class DeleteCustomerCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly MovieStoreDbContext _dbContext;

    public DeleteCustomerCommandTests(CommonTestFixture testFixture)
    {
        _dbContext = testFixture.Context;
    }

    [Fact]
    public void WhenGivenCustomerIsNotFound_Handle_ThrowsInvalidOperationException()
    {
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        var claims = new List<Claim>()
        {
            new Claim("customerId", "1")
        };
        mockHttpContextAccessor.Setup(accessor => accessor.HttpContext.User.Claims).Returns(claims);

        DeleteCustomerCommand command = new DeleteCustomerCommand(_dbContext, mockHttpContextAccessor.Object);
        command.Id = 999;

        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("No found customer.");
    }

    [Fact]
    public void WhenGivenCustomerIdIsNotTheSameWithTheCustomerIdInToken_Handle_ThrowsInvalidOperationException()
    {
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        var claims = new List<Claim>()
        {
            new Claim("customerId", "1")
        };
        mockHttpContextAccessor.Setup(accessor => accessor.HttpContext.User.Claims).Returns(claims);

        DeleteCustomerCommand command = new DeleteCustomerCommand(_dbContext, mockHttpContextAccessor.Object);
        command.Id = 2;

        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("You can only delete your own account.");
    }

    [Fact]
    public void WhenValidInputsAreGiven_Customer_ShouldBeDeleted()
    {
        Customer customer = new Customer()
        {
            FirstName = "A New", LastName = "Customer To Delete", Email = "customertodelete@example.com",
            Password = "customertodelete",
        };
        _dbContext.Customers.Add(customer);
        _dbContext.SaveChanges();

        Customer newCustomer = _dbContext.Customers.SingleOrDefault(c => c.Email.ToLower() == customer.Email.ToLower());

        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        var claims = new List<Claim>()
        {
            new Claim("customerId", Convert.ToString(newCustomer.Id))
        };
        mockHttpContextAccessor.Setup(accessor => accessor.HttpContext.User.Claims).Returns(claims);

        DeleteCustomerCommand command = new DeleteCustomerCommand(_dbContext, mockHttpContextAccessor.Object);
        command.Id = newCustomer.Id;

        FluentActions.Invoking(() => command.Handle()).Invoke();

        Customer deletedCustomer =
            _dbContext.Customers.SingleOrDefault(c => c.Email.ToLower() == newCustomer.Email.ToLower());

        deletedCustomer.Should().BeNull();
    }
}