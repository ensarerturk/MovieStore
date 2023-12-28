using FluentValidation;

namespace MovieStore.Application.DirectorOperations.Queries.GetDirectorById;

public class GetDirectorByIdQueryValidator : AbstractValidator<GetDirectorByIdQuery>
{
    public GetDirectorByIdQueryValidator()
    {
        RuleFor(query => query.Id).GreaterThan(0);
    }
}