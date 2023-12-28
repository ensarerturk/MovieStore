using MovieStore.DBOperations;
using MovieStore.Entities;

namespace MovieStore.Application.DirectorOperations.Commands.UpdateDirector;

public class UpdateDirectorCommand
{
    public int Id { get; set; }
    public UpdateDirectorModel Model { get; set; }
    private readonly IMovieStoreDbContext _dbContext;

    public UpdateDirectorCommand(IMovieStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Handle()
    {
        Director director = _dbContext.Directors.SingleOrDefault(director => director.Id == Id);
        if (director is null)
        {
            throw new InvalidOperationException("No found director.");
        }

        director.FirstName = string.IsNullOrEmpty(Model.FirstName) ? director.FirstName : Model.FirstName.Trim();
        director.LastName = string.IsNullOrEmpty(Model.LastName) ? director.LastName : Model.LastName.Trim();

        if (_dbContext.Directors.Any(d =>
                d.FirstName.ToLower() == director.FirstName.ToLower() &&
                d.LastName.ToLower() == director.LastName.ToLower() && d.Id != director.Id))
        {
            throw new InvalidOperationException("There is already a director with that name.");
        }

        _dbContext.SaveChanges();
    }
}

public class UpdateDirectorModel
{
    private string firstName;

    public string FirstName
    {
        get { return firstName; }
        set { firstName = value.Trim(); }
    }

    private string lastName;

    public string LastName
    {
        get { return lastName; }
        set { lastName = value.Trim(); }
    }
}