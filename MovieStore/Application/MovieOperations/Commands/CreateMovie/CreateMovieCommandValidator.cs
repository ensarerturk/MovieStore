using FluentValidation;

namespace MovieStore.Application.MovieOperations.Commands.CreateMovie;

public class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
{
    public CreateMovieCommandValidator()
    {
        RuleFor(command => command.Model.GenreId).GreaterThan(0);
        RuleFor(command => command.Model.DirectorId).GreaterThan(0);
        RuleFor(command => command.Model.Name).NotEmpty().MinimumLength(1);
        RuleFor(command => command.Model.ReleaseYear).GreaterThan(0).LessThan(DateTime.Now.AddYears(1).Year);
        RuleFor(command => command.Model.Price).GreaterThan(0);
    }
}