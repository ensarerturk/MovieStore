using AutoMapper;
using MovieStore.DBOperations;
using MovieStore.Entities;

namespace MovieStore.Application.DirectorOperations.Commands.CreateDirector;

public class CreateDirectorCommand
{
    public CreateDirectorModel Model { get; set; }

    private readonly IMovieStoreDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateDirectorCommand(IMovieStoreDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public void Handle()
    {
        Director director = _dbContext.Directors.SingleOrDefault(director =>
            (director.FirstName == Model.FirstName && director.LastName == Model.LastName));
        if (director is not null)
        {
            throw new InvalidOperationException("The director already exists.");
        }

        director = _mapper.Map<Director>(Model);

        _dbContext.Directors.Add(director);
        _dbContext.SaveChanges();
    }
}

public class CreateDirectorModel
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
}