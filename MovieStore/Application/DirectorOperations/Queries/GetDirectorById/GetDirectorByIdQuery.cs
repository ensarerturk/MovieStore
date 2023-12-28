using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieStore.Application.DirectorOperations.Queries.SharedViewModels;
using MovieStore.DBOperations;
using MovieStore.Entities;

namespace MovieStore.Application.DirectorOperations.Queries.GetDirectorById;

public class GetDirectorByIdQuery
{
    public int Id { get; set; }
    private readonly IMovieStoreDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetDirectorByIdQuery(IMovieStoreDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public GetDirectorByIdViewModel Handle()
    {
        Director director = _dbContext.Directors.Where(director => director.Id == Id)
            .Include(director => director.DirectedMovies.Where(movie => movie.isActive))
            .ThenInclude(movie => movie.Director)
            .Include(director => director.DirectedMovies.Where(movie => movie.isActive))
            .ThenInclude(movie => movie.Genre)
            .SingleOrDefault() ?? throw new InvalidOperationException();

        if (director is null)
        {
            throw new InvalidOperationException("No found director.");
        }

        GetDirectorByIdViewModel directorVM = _mapper.Map<GetDirectorByIdViewModel>(director);
        return directorVM;
    }
}

public class GetDirectorByIdViewModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<DirectedMovieViewModel> DirectedMovies { get; set; }
}