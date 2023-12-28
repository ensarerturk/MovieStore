using FluentValidation;

namespace MovieStore.Application.MovieOperations.Commands.UpdateMovie;

public class UpdateMovieCommandValidator : AbstractValidator<UpdateMovieCommand>
{
    public UpdateMovieCommandValidator()
    {
        RuleFor(command => command.Id).GreaterThan(0);
        RuleFor(command => command.Model.GenreId).GreaterThan(-1);
        RuleFor(command => command.Model.DirectorId).GreaterThan(-1);
        RuleFor(command => command.Model.Name).MinimumLength(1).When(command => command.Model.Name != string.Empty);
        RuleFor(command => command.Model.ReleaseYear).GreaterThan(-1).LessThan(DateTime.Now.AddYears(1).Year);
        RuleFor(command => command.Model.Price).GreaterThan(-1);
    }
}