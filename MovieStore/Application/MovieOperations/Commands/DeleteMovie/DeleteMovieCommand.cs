using MovieStore.DBOperations;
using MovieStore.Entities;

namespace MovieStore.Application.MovieOperations.Commands.DeleteMovie;

public class DeleteMovieCommand
{
    public int Id { get; set; }
    private readonly IMovieStoreDbContext _dbContext;

    public DeleteMovieCommand(IMovieStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Handle()
    {
        Movie movie = _dbContext.Movies.SingleOrDefault(movie => movie.Id == Id && movie.isActive);
        if (movie is null)
        {
            throw new InvalidOperationException("No found movie.");
        }

        movie.isActive = false;
        _dbContext.SaveChanges();
    }
}