using AutoMapper;
using MovieStore.DBOperations;
using MovieStore.Entities;

namespace MovieStore.Application.MovieOperations.Commands.CreateMovie;

public class CreateMovieCommand
{
    public CreateMovieModel Model { get; set; }

    private readonly IMovieStoreDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateMovieCommand(IMovieStoreDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public void Handle()
    {
        Movie movie = _dbContext.Movies.SingleOrDefault(movie => movie.Name == Model.Name);
        if (movie is not null)
        {
            throw new InvalidOperationException("The movie already exist.");
        }

        movie = _mapper.Map<Movie>(Model);

        _dbContext.Movies.Add(movie);
        _dbContext.SaveChanges();
    }
}

public class CreateMovieModel
{
    private string name;

    public string Name
    {
        get { return name; }
        set { name = value.Trim(); }
    }

    public int GenreId { get; set; }
    public int DirectorId { get; set; }
    public int ReleaseYear { get; set; }
    public int Price { get; set; }
}