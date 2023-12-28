using FluentValidation;

namespace MovieStore.Application.MovieOperations.Commands.BuyMovie;

public class BuyMovieCommandValidator : AbstractValidator<BuyMovieCommand>
{
    public BuyMovieCommandValidator()
    {
        RuleFor(command => command.Id).GreaterThan(0);
    }
}