using Microsoft.EntityFrameworkCore;
using MovieStore.DBOperations;
using MovieStore.Entities;

namespace MovieStore.Application.MovieOperations.Commands.AddActor;

public class AddActorCommand
{
    public int Id { get; set; }
    public AddActorModel Model { get; set; }
    private readonly IMovieStoreDbContext _dbContext;

    public AddActorCommand(IMovieStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Handle()
    {
        Movie movie = _dbContext.Movies.Include(movie => movie.Actors)
            .SingleOrDefault(movie => movie.Id == Id && movie.isActive);
        if (movie is null)
        {
            throw new InvalidOperationException("No found movie.");
        }

        Actor actor = _dbContext.Actors.Include(actor => actor.Movies)
            .SingleOrDefault(actor => actor.Id == Model.ActorId);
        if (actor is null)
        {
            throw new InvalidOperationException("No found actor.");
        }

        if (movie.Actors.Any(actor => actor.Id == Model.ActorId))
        {
            throw new InvalidOperationException("This actor is already in this movie.");
        }

        actor.Movies.Add(movie);
        _dbContext.SaveChanges();
    }
}

public class AddActorModel
{
    public int ActorId { get; set; }
}