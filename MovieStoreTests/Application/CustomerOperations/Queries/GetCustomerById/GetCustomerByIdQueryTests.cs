using AutoMapper;
using FluentAssertions;
using MovieStore.Application.CustomerOperations.Queries.GetCustomerById;
using MovieStore.DBOperations;
using MovieStore.Entities;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.CustomerOperations.Queries.GetCustomerById;

public class GetCustomerByIdQueryTests : IClassFixture<CommonTestFixture>
{
    private readonly MovieStoreDbContext _context;
    private readonly IMapper _mapper;

    public GetCustomerByIdQueryTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

    [Fact]
    public void WhenGivenCustomerIsNotFound_Handle_ThrowsInvalidOperationException()
    {
        // arrange
        GetCustomerByIdQuery query = new GetCustomerByIdQuery(_context, _mapper);
        query.Id = 999;

        // act & assert
        FluentActions
            .Invoking(() => query.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("Müşteri bulunamadı.");
    }

    [Fact]
    public void WhenValidInputsAreGiven_Customer_ShouldBeReturned()
    {
        // arrange
        GetCustomerByIdQuery query = new GetCustomerByIdQuery(_context, _mapper);
        query.Id = 2;

        // act
        FluentActions.Invoking(() => query.Handle()).Invoke();

        // assert
        Customer customer = _context.Customers.SingleOrDefault(customer => customer.Id == query.Id);

        customer.Should().NotBeNull();
    }
}