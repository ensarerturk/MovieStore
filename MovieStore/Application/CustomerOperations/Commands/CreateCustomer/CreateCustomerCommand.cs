using AutoMapper;
using MovieStore.DBOperations;
using MovieStore.Entities;

namespace MovieStore.Application.CustomerOperations.Commands.CreateCustomer;

public class CreateCustomerCommand
{
    public CreateCustomerModel Model { get; set; }
    private readonly IMovieStoreDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateCustomerCommand(IMovieStoreDbContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public void Handle()
    {
        Customer customer = _dbContext.Customers.SingleOrDefault(customer => customer.Email == Model.Email);
        if (customer is not null)
        {
            throw new InvalidOperationException("The customer already exists.");
        }

        customer = _mapper.Map<Customer>(Model);

        _dbContext.Customers.Add(customer);
        _dbContext.SaveChanges();
    }
}

public class CreateCustomerModel
{
    private string firstName;

    public string FirstName
    {
        get { return firstName; }
        set { firstName = value.Trim(); }
    }

    private string lastName;

    public string LastName
    {
        get { return lastName; }
        set { lastName = value.Trim(); }
    }

    private string email;

    public string Email
    {
        get { return email; }
        set { email = value.Trim(); }
    }

    public string Password { get; set; }
}