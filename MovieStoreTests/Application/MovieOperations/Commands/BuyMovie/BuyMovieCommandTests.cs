using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using MovieStore.Application.MovieOperations.Commands.BuyMovie;
using MovieStore.DBOperations;
using MovieStore.Entities;
using MovieStoreTests.TestSetup;

namespace MovieStoreTests.Application.MovieOperations.Commands.BuyMovie;

public class BuyMovieCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly MovieStoreDbContext _dbContext;

    public BuyMovieCommandTests(CommonTestFixture testFixture)
    {
        _dbContext = testFixture.Context;
    }

    [Fact]
    public void WhenGivenMovieIsNotFound_Handle_ThrowsInvalidOperationException()
    {
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        var claims = new List<Claim>()
        {
            new Claim("customerId", "1")
        };
        mockHttpContextAccessor.Setup(accessor => accessor.HttpContext.User.Claims).Returns(claims);

        BuyMovieCommand command = new BuyMovieCommand(_dbContext, mockHttpContextAccessor.Object);
        command.Id = 999;

        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("No found movie.");
    }

    [Fact]
    public void WhenGivenMovieIsNotActive_Handle_ThrowsInvalidOperationException()
    {
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        var claims = new List<Claim>()
        {
            new Claim("customerId", "1")
        };

        mockHttpContextAccessor.Setup(accessor => accessor.HttpContext.User.Claims).Returns(claims);
        Movie newMovieToTestNotActive = new Movie()
        {
            Name = "test name 12145445124124", ReleaseYear = 2000, Price = 15, GenreId = 4, DirectorId = 1,
            isActive = false
        };
        _dbContext.Movies.Add(newMovieToTestNotActive);
        _dbContext.SaveChanges();
        Movie addedMovieToTestNotActive =
            _dbContext.Movies.SingleOrDefault(movie => movie.Name == newMovieToTestNotActive.Name);
        // Arrange
        BuyMovieCommand command = new BuyMovieCommand(_dbContext, mockHttpContextAccessor.Object);
        command.Id = addedMovieToTestNotActive.Id;

        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("No found movie.");
    }

    [Fact]
    public void WhenValidInputsAreGiven_Movie_ShouldBeAddedToUsersMovies()
    {
        Customer customer = new Customer()
        {
            FirstName = "New", LastName = "Customer", Email = "newcustomer@example.com", Password = "newcustomer123"
        };
        _dbContext.Customers.Add(customer);
        _dbContext.SaveChanges();

        Customer newCustomer = _dbContext.Customers.Include(customer => customer.Orders).SingleOrDefault(c =>
            c.FirstName.ToLower() == customer.FirstName.ToLower() &&
            c.LastName.ToLower() == customer.LastName.ToLower());

        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        var claims = new List<Claim>()
        {
            new Claim("customerId", Convert.ToString(newCustomer.Id))
        };
        mockHttpContextAccessor.Setup(accessor => accessor.HttpContext.User.Claims).Returns(claims);

        BuyMovieCommand command = new BuyMovieCommand(_dbContext, mockHttpContextAccessor.Object);
        command.Id = 1;

        FluentActions.Invoking(() => command.Handle()).Invoke();

        Customer customerAfterOrder = _dbContext.Customers.Include(customer => customer.Orders)
            .SingleOrDefault(c => c.Id == newCustomer.Id);
        bool hasNewMovie = customerAfterOrder.Orders.Any(order => order.MovieId == command.Id);

        customerAfterOrder.Orders.Count.Should().BeGreaterThan(0);
        hasNewMovie.Should().BeTrue();
    }
}