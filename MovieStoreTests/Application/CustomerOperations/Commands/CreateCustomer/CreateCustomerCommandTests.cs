using AutoMapper;
using FluentAssertions;
using MovieStore.Application.CustomerOperations.Commands.CreateCustomer;
using MovieStore.DBOperations;
using MovieStore.Entities;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.CustomerOperations.Commands.CreateCustomer;

public class CreateCustomerCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly MovieStoreDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateCustomerCommandTests(CommonTestFixture testFixture)
    {
        _dbContext = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

    [Fact]
    public void Handle_ThrowsInvalidOperationException_WhenAlreadyExistingCustomerEmailIsGiven()
    {
        // Arrange
        Customer existingCustomer = new()
        {
            Email = "existing@example.com",
            Password = "existing123",
            FirstName = "existingfn",
            LastName = "existingln",
        };
        _dbContext.Customers.Add(existingCustomer);
        _dbContext.SaveChanges();

        CreateCustomerCommand command = new(_dbContext, _mapper);
        command.Model = new CreateCustomerModel()
        {
            Email = "existing@example.com",
            Password = "existing123",
            FirstName = "existingfn",
            LastName = "existingln",
        };

        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("The customer already exists.");
    }

    [Fact]
    public void Handle_ShouldCreateCustomer_WhenValidInputsAreGiven()
    {
        // Arrange
        CreateCustomerCommand command = new(_dbContext, _mapper);
        var model = new CreateCustomerModel()
        {
            Email = "new@example.com",
            Password = "new123123",
            FirstName = "newcustomer",
            LastName = "newcustomerln",
        };
        command.Model = model;

        // Act
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // Assert
        var customer = _dbContext.Customers.SingleOrDefault(c => c.Email.ToLower() == model.Email.ToLower());

        customer.Should().NotBeNull();
    }
}