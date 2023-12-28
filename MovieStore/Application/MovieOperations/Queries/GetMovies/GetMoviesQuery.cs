using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieStore.Application.MovieOperations.Queries.SharedViewModels;
using MovieStore.DBOperations;
using MovieStore.Entities;

namespace MovieStore.Application.MovieOperations.Queries.GetMovies;

public class GetMoviesQuery
{
    private readonly IMovieStoreDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetMoviesQuery(IMovieStoreDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public List<MoviesViewModel> Handle()
    {
        List<Movie> movies = _dbContext.Movies.Where(movie => movie.isActive).Include(movie => movie.Director)
            .Include(movie => movie.Actors).Include(movie => movie.Genre).OrderBy(movie => movie.Id).ToList<Movie>();
        List<MoviesViewModel> moviesVM = _mapper.Map<List<MoviesViewModel>>(movies);
        return moviesVM;
    }
}

public class MoviesViewModel
{
    public string Name { get; set; }
    public int ReleaseYear { get; set; }
    public string Genre { get; set; }
    public string Director { get; set; }
    public int Price { get; set; }
    public List<ActorViewModel> Actors { get; set; }
}