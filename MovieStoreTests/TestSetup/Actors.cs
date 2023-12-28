using MovieStore.DBOperations;
using MovieStore.Entities;

namespace MovieStoreTests.TestSetup;

public static class Actors
{
    public static void AddActors(this MovieStoreDbContext context)
    {
        var movie1 = new Movie { Name = "Pulp Fiction", GenreId = 1, DirectorId = 1, Price = 10, ReleaseYear = 1992 };
        var movie2 = new Movie
            { Name = "Catch Me If You Can", GenreId = 2, DirectorId = 2, Price = 12, ReleaseYear = 1990 };
        var movie3 = new Movie { Name = "Fight Club", GenreId = 3, DirectorId = 3, Price = 14, ReleaseYear = 2002 };

        context.Actors.AddRange(
            new Actor { FirstName = "Alison", LastName = "Brie", Movies = new List<Movie> { movie1, movie2 } },
            new Actor { FirstName = "Danny", LastName = "Pudi", Movies = new List<Movie> { movie1, movie3 } },
            new Actor { FirstName = "Donald", LastName = "Glover", Movies = new List<Movie> { movie2, movie3 } }
        );
    }
}