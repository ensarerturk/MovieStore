using System.ComponentModel.DataAnnotations.Schema;

namespace MovieStore.Entities;

public class Actor
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<Movie> Movies { get; set; }
}