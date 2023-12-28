using Microsoft.EntityFrameworkCore;
using MovieStore.Entities;

namespace MovieStore.DBOperations;

public class GenreGenerator
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context =
               new MovieStoreDbContext(serviceProvider.GetRequiredService<DbContextOptions<MovieStoreDbContext>>()))
        {
            if (context.Genres.Any())
            {
                return;
            }

            var genre1 = new Genre { Name = "Action" };
            var genre2 = new Genre { Name = "Drama" };
            var genre3 = new Genre { Name = "Horror" };
            var genre4 = new Genre { Name = "Comedy" };

            context.Genres.AddRange(genre1, genre2, genre3, genre4);

            context.SaveChanges();
        }
    }
}