using FluentValidation;

namespace MovieStore.Application.MovieOperations.Queries.GetMovieById;

public class GetMovieByIdQueryValidator : AbstractValidator<GetMovieByIdQuery>
{
    public GetMovieByIdQueryValidator()
    {
        RuleFor(query => query.Id).GreaterThan(0);
    }
}