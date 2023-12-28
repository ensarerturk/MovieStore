using Microsoft.EntityFrameworkCore;
using MovieStore.DBOperations;
using MovieStore.Entities;

namespace MovieStore.Application.DirectorOperations.Commands.DeleteDirector;

public class DeleteDirectorCommand
{
    public int Id { get; set; }
    private readonly IMovieStoreDbContext _dbContext;

    public DeleteDirectorCommand(IMovieStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Handle()
    {
        Director director = _dbContext.Directors.SingleOrDefault(director => director.Id == Id);
        if (director is null)
        {
            throw new InvalidOperationException("No found director");
        }

        bool isDirectingAnyMovie = _dbContext.Movies.Include(movie => movie.Director)
            .Any(movie => movie.isActive && movie.Director.Id == director.Id);

        if (isDirectingAnyMovie)
        {
            throw new InvalidOperationException("This director is directing a movie, it cannot be deleted now.");
        }

        _dbContext.Directors.Remove(director);
        _dbContext.SaveChanges();
    }
}