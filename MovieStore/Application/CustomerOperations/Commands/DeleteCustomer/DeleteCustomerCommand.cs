using Microsoft.EntityFrameworkCore;
using MovieStore.DBOperations;
using MovieStore.Entities;

namespace MovieStore.Application.CustomerOperations.Commands.DeleteCustomer;

public class DeleteCustomerCommand
{
    public int Id { get; set; }
    private readonly IMovieStoreDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteCustomerCommand(IMovieStoreDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public void Handle()
    {
        Customer customer = _dbContext.Customers.Include(customer => customer.Orders)
            .SingleOrDefault(customer => customer.Id == Id);
        if (customer is null)
        {
            throw new InvalidOperationException("No customer found.");
        }

        int requestOwnerId = int.Parse(_httpContextAccessor.HttpContext.User.Claims
            .SingleOrDefault(claim => claim.Type == "customerId").Value);
        if (requestOwnerId != customer.Id)
        {
            throw new InvalidOperationException("You can only delete your own account.");
        }

        _dbContext.Customers.Remove(customer);
        _dbContext.SaveChanges();
    }
}