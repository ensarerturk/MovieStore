using Microsoft.EntityFrameworkCore;
using MovieStore.DBOperations;
using MovieStore.Entities;

namespace MovieStore.Application.ActorOperations.Commands.DeleteActor;

public class DeleteActorCommand
{
    public int Id { get; set; }
    private readonly IMovieStoreDbContext _dbContext;

    public DeleteActorCommand(IMovieStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Handle()
    {
        Actor actor = _dbContext.Actors.SingleOrDefault(actor => actor.Id == Id);
        if (actor is null)
        {
            throw new InvalidOperationException("Oyuncu bulunamadı.");
        }

        bool isPlayingInAnyMovie = _dbContext.Movies.Include(movie => movie.Actors)
            .Any(movie => movie.isActive && movie.Actors.Any(a => a.Id == actor.Id));

        if (isPlayingInAnyMovie)
        {
            throw new InvalidOperationException("This actor is playing in a movie, which cannot be deleted at the moment.");
        }

        _dbContext.Actors.Remove(actor);
        _dbContext.SaveChanges();
    }
}