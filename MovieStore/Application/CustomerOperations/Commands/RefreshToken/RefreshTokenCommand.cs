using MovieStore.DBOperations;
using MovieStore.Entities;
using MovieStore.TokenOperations;
using MovieStore.TokenOperations.Models;

namespace MovieStore.Application.CustomerOperations.Commands.RefreshToken;

public class RefreshTokenCommand
{
    public string RefreshToken { get; set; }
    private readonly IMovieStoreDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public RefreshTokenCommand(IMovieStoreDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public Token Handle()
    {
        Customer customer = _dbContext.Customers.FirstOrDefault(customer =>
            customer.RefreshToken == RefreshToken && customer.RefreshTokenExpireDate > DateTime.Now);
        if (customer is not null)
        {
            TokenHandler handler = new TokenHandler(_configuration);
            Token token = handler.CreateAccessToken(customer);

            customer.RefreshToken = token.RefreshToken;
            customer.RefreshTokenExpireDate = token.ExpirationDate.AddMinutes(5);
            _dbContext.SaveChanges();

            return token;
        }
        else
        {
            throw new InvalidOperationException("No valid Refresh Token found.");
        }
    }
}