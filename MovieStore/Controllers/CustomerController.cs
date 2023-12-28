using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Application.CustomerOperations.Commands.CreateCustomer;
using MovieStore.Application.CustomerOperations.Commands.CreateToken;
using MovieStore.Application.CustomerOperations.Commands.DeleteCustomer;
using MovieStore.Application.CustomerOperations.Commands.RefreshToken;
using MovieStore.Application.CustomerOperations.Queries.GetCustomerById;
using MovieStore.Application.CustomerOperations.Queries.GetCustomers;
using MovieStore.DBOperations;
using MovieStore.TokenOperations.Models;

namespace MovieStore.Controllers;

[ApiController]
[Route("[Controller]s")]
public class CustomerController : ControllerBase
{
    private readonly IMovieStoreDbContext _context;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomerController(IMovieStoreDbContext context, IMapper mapper, IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    public IActionResult CreateCustomer([FromBody] CreateCustomerModel newCustomer)
    {
        CreateCustomerCommand command = new CreateCustomerCommand(_context, _mapper);
        command.Model = newCustomer;

        CreateCustomerCommandValidator validator = new CreateCustomerCommandValidator();
        validator.ValidateAndThrow(command);

        command.Handle();

        return Ok();
    }

    [HttpPost("connect/token")]
    public ActionResult<Token> CreateToken([FromBody] LoginModel loginInfo)
    {
        CreateTokenCommand command = new CreateTokenCommand(_context, _configuration);
        command.Model = loginInfo;

        CreateTokenCommandValidator validator = new CreateTokenCommandValidator();
        validator.ValidateAndThrow(command);

        Token token = command.Handle();

        return token;
    }

    [HttpGet("refreshToken")]
    public ActionResult<Token> RefreshToken([FromQuery] string token)
    {
        RefreshTokenCommand command = new RefreshTokenCommand(_context, _configuration);
        command.RefreshToken = token;
        Token resultAccessToken = command.Handle();
        return resultAccessToken;
    }

    [HttpGet]
    public IActionResult GetCustomers()
    {
        GetCustomersQuery query = new GetCustomersQuery(_context, _mapper);
        List<CustomerViewModel> result = query.Handle();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetCustomerById(int id)
    {
        GetCustomerByIdQuery query = new GetCustomerByIdQuery(_context, _mapper);
        query.Id = id;

        GetCustomerByIdQueryValidator validator = new GetCustomerByIdQueryValidator();
        validator.ValidateAndThrow(query);

        GetCustomerByIdViewModel result = query.Handle();

        return Ok(result);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public IActionResult DeleteCustomer(int id)
    {
        DeleteCustomerCommand command = new DeleteCustomerCommand(_context, _httpContextAccessor);
        command.Id = id;

        DeleteCustomerCommandValidator validator = new DeleteCustomerCommandValidator();
        validator.ValidateAndThrow(command);

        command.Handle();

        return Ok();
    }
}