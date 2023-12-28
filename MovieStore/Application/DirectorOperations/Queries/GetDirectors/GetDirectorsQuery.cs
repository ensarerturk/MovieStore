using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieStore.Application.DirectorOperations.Queries.SharedViewModels;
using MovieStore.DBOperations;
using MovieStore.Entities;

namespace MovieStore.Application.DirectorOperations.Queries.GetDirectors;

public class GetDirectorsQuery
{
    private readonly IMovieStoreDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetDirectorsQuery(IMovieStoreDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public List<DirectorsViewModel> Handle()
    {
        List<Director> directors = _dbContext.Directors
            .Include(director => director.DirectedMovies.Where(movie => movie.isActive))
            .ThenInclude(movie => movie.Genre).OrderBy(director => director.Id).ToList<Director>();
        List<DirectorsViewModel> directorsVM = _mapper.Map<List<DirectorsViewModel>>(directors);
        return directorsVM;
    }
}

public class DirectorsViewModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<DirectedMovieViewModel> DirectedMovies { get; set; }
}