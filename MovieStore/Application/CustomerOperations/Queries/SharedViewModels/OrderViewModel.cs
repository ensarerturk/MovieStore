namespace MovieStore.Application.CustomerOperations.Queries.SharedViewModels;

public class OrderViewModel
{
    public string Movie { get; set; }
    public int Price { get; set; }
    public DateTime ProcessDate { get; set; }
}