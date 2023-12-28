using Microsoft.EntityFrameworkCore;
using MovieStore.DBOperations;
using MovieStore.Entities;

namespace MovieStore.Application.MovieOperations.Commands.BuyMovie;

public class BuyMovieCommand
{
    public int Id { get; set; }
    private readonly IMovieStoreDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BuyMovieCommand(IMovieStoreDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public void Handle()
    {
        Movie movie = _dbContext.Movies.SingleOrDefault(movie => movie.Id == Id && movie.isActive);
        if (movie is null)
        {
            throw new InvalidOperationException("No found movie.");
        }

        int customerId = int.Parse(_httpContextAccessor.HttpContext.User.Claims
            .SingleOrDefault(claim => claim.Type == "customerId").Value);
        Order order = new Order
            { CustomerId = customerId, MovieId = movie.Id, Price = movie.Price, ProcessDate = DateTime.Now };
        Customer customer = _dbContext.Customers.Include(customer => customer.Orders)
            .SingleOrDefault(customer => customer.Id == customerId);
        customer.Orders.Add(order);
        _dbContext.SaveChanges();
    }
}