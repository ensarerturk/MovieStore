using MovieStore.DBOperations;
using MovieStore.Entities;

namespace MovieStoreTests.TestSetup;

public static class Customers
{
    public static void AddCustomers(this MovieStoreDbContext context)
    {
        var genre1 = new Genre { Name = "Action" };
        var genre2 = new Genre { Name = "Fantasy" };
        var genre3 = new Genre { Name = "Drama" };
        var genre4 = new Genre { Name = "Sci-Fi" };
        var genre5 = new Genre { Name = "Romance" };
        var genre6 = new Genre { Name = "Horror" };
        var genre7 = new Genre { Name = "Comedy" };

        var movie1 = new Movie { Name = "Pulp Fiction", GenreId = 1, DirectorId = 1, Price = 10, ReleaseYear = 1992 };
        var movie2 = new Movie
            { Name = "Catch Me If You Can", GenreId = 2, DirectorId = 2, Price = 12, ReleaseYear = 1990 };
        var movie3 = new Movie { Name = "Fight Club", GenreId = 3, DirectorId = 3, Price = 14, ReleaseYear = 2002 };

        var order1 = new Order
            { CustomerId = 1, MovieId = 1, Price = movie1.Price, ProcessDate = DateTime.Now.AddDays(-10) };
        var order2 = new Order
            { CustomerId = 1, MovieId = 1, Price = movie1.Price, ProcessDate = DateTime.Now.AddDays(-15) };
        var order3 = new Order
            { CustomerId = 1, MovieId = 3, Price = movie3.Price, ProcessDate = DateTime.Now.AddDays(-20) };
        var order4 = new Order
            { CustomerId = 2, MovieId = 1, Price = movie1.Price, ProcessDate = DateTime.Now.AddDays(-10) };
        var order5 = new Order
            { CustomerId = 2, MovieId = 2, Price = movie2.Price, ProcessDate = DateTime.Now.AddDays(-15) };
        var order6 = new Order
            { CustomerId = 2, MovieId = 3, Price = movie3.Price, ProcessDate = DateTime.Now.AddDays(-20) };
        var order7 = new Order
            { CustomerId = 3, MovieId = 1, Price = movie1.Price, ProcessDate = DateTime.Now.AddDays(-10) };
        var order8 = new Order
            { CustomerId = 3, MovieId = 2, Price = movie2.Price, ProcessDate = DateTime.Now.AddDays(-15) };
        var order9 = new Order
            { CustomerId = 3, MovieId = 3, Price = movie3.Price, ProcessDate = DateTime.Now.AddDays(-20) };

        context.Customers.AddRange(
            new Customer
            {
                FirstName = "Furkan", LastName = "Setbaşı", Email = "furkan@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("furkan123"),
                Orders = new List<Order> { order1, order2, order3 }, FavoriteGenres = new List<Genre> { genre1, genre2 }
            },
            new Customer
            {
                FirstName = "Hakan", LastName = "Çelik", Email = "hakan@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("hakan123"),
                Orders = new List<Order> { order4, order5, order6 },
                FavoriteGenres = new List<Genre> { genre2, genre5, genre7, genre3 }
            },
            new Customer
            {
                FirstName = "Kemal", LastName = "Yalçın", Email = "kemal@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("kemal123"),
                Orders = new List<Order> { order7, order8, order9 },
                FavoriteGenres = new List<Genre> { genre7, genre6, genre4 }
            }
        );
    }
}