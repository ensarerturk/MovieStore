using MovieStore.DBOperations;
using MovieStore.Entities;

namespace MovieStore.Application.MovieOperations.Commands.UpdateMovie;

public class UpdateMovieCommand
{
    public int Id { get; set; }
    public UpdateMovieModel Model { get; set; }
    private readonly IMovieStoreDbContext _dbContext;

    public UpdateMovieCommand(IMovieStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Handle()
    {
        Movie movie = _dbContext.Movies.SingleOrDefault(movie => movie.Id == Id && movie.isActive);
        if (movie is null)
        {
            throw new InvalidOperationException("No found movies.");
        }

        movie.GenreId = Model.GenreId != default ? Model.GenreId : movie.GenreId;
        movie.DirectorId = Model.DirectorId != default ? Model.DirectorId : movie.DirectorId;
        movie.ReleaseYear = Model.ReleaseYear != default ? Model.ReleaseYear : movie.ReleaseYear;
        movie.Name = string.IsNullOrEmpty(Model.Name) ? movie.Name : Model.Name.Trim();
        movie.Price = Model.Price != default ? Model.Price : movie.Price;

        if (_dbContext.Movies.Any(m => m.Name.ToLower() == movie.Name.ToLower() && m.Id != movie.Id))
        {
            throw new InvalidOperationException("There's already a movie with that name.");
        }

        _dbContext.SaveChanges();
    }
}

public class UpdateMovieModel
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