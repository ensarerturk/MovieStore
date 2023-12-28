using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieStore.Application.MovieOperations.Queries.SharedViewModels;
using MovieStore.DBOperations;
using MovieStore.Entities;

namespace MovieStore.Application.MovieOperations.Queries.GetMovieById;

public class GetMovieByIdQuery
{
    public int Id { get; set; }
    private readonly IMovieStoreDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetMovieByIdQuery(IMovieStoreDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public GetMovieByIdViewModel Handle()
    {
        Movie movie = _dbContext.Movies.Where(movie => movie.Id == Id && movie.isActive).Include(movie => movie.Genre)
            .Include(movie => movie.Director).Include(movie => movie.Actors).SingleOrDefault();
        if (movie is null)
        {
            throw new InvalidOperationException("No found movie.");
        }

        GetMovieByIdViewModel movieVM = _mapper.Map<GetMovieByIdViewModel>(movie);
        return movieVM;
    }
}

public class GetMovieByIdViewModel
{
    public string Name { get; set; }
    public int ReleaseYear { get; set; }
    public string Genre { get; set; }
    public string Director { get; set; }
    public int Price { get; set; }
    public List<ActorViewModel> Actors { get; set; }
}