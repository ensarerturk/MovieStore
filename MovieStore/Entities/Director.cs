using System.ComponentModel.DataAnnotations.Schema;

namespace MovieStore.Entities;

public class Director
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<Movie> DirectedMovies { get; set; }
}