using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieStore.Application.CustomerOperations.Queries.SharedViewModels;
using MovieStore.DBOperations;
using MovieStore.Entities;

namespace MovieStore.Application.CustomerOperations.Queries.GetCustomers;

public class GetCustomersQuery
{
    private readonly IMovieStoreDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetCustomersQuery(IMovieStoreDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public List<CustomerViewModel> Handle()
    {
        List<Customer> customers = _dbContext.Customers.Include(customer => customer.Orders)
            .ThenInclude(order => order.Movie).Include(customer => customer.FavoriteGenres)
            .OrderBy(customer => customer.Id).ToList<Customer>();
        List<CustomerViewModel> customersVM = _mapper.Map<List<CustomerViewModel>>(customers);

        return customersVM;
    }
}

public class CustomerViewModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<OrderViewModel> Orders { get; set; }
    public List<GenreViewModel> FavoriteGenres { get; set; }
}