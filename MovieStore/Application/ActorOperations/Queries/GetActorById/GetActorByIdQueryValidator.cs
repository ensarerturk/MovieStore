using FluentValidation;

namespace MovieStore.Application.ActorOperations.Queries.GetActorById;

public class GetActorByIdQueryValidator : AbstractValidator<GetActorByIdQuery>
{
    public GetActorByIdQueryValidator()
    {
        RuleFor(query => query.Id).GreaterThan(0);
    }
}