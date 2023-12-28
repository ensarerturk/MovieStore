using FluentValidation;

namespace MovieStore.Application.MovieOperations.Commands.DeleteMovie;

public class DeleteMovieCommandValidator : AbstractValidator<DeleteMovieCommand>
{
    public DeleteMovieCommandValidator()
    {
        RuleFor(command => command.Id).GreaterThan(0);
    }
}