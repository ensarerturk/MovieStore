using System.ComponentModel.DataAnnotations.Schema;

namespace MovieStore.Entities;

public class Genre
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; }
    public List<Customer> Customers { get; set; }
}