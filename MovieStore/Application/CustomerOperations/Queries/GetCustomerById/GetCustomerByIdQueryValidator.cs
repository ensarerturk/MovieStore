using FluentValidation;

namespace MovieStore.Application.CustomerOperations.Queries.GetCustomerById;

public class GetCustomerByIdQueryValidator : AbstractValidator<GetCustomerByIdQuery>
{
    public GetCustomerByIdQueryValidator()
    {
        RuleFor(query => query.Id).GreaterThan(0);
    }
}