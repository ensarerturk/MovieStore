using MovieStore.DBOperations;
using MovieStore.Entities;
using MovieStore.TokenOperations;
using MovieStore.TokenOperations.Models;

namespace MovieStore.Application.CustomerOperations.Commands.CreateToken;

public class CreateTokenCommand
{
    public LoginModel Model { get; set; }
    private readonly IMovieStoreDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public CreateTokenCommand(IMovieStoreDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public Token Handle()
    {
        Customer customer = _dbContext.Customers.FirstOrDefault(customer => customer.Email == Model.Email);

        if (customer is not null && BCrypt.Net.BCrypt.Verify(Model.Password, customer.Password))
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
            throw new InvalidOperationException("The username or password is incorrect.");
        }
    }
}

public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}