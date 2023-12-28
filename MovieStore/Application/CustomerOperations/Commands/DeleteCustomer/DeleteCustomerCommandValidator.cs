using FluentValidation;

namespace MovieStore.Application.CustomerOperations.Commands.DeleteCustomer;

public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
{
    public DeleteCustomerCommandValidator()
    {
        RuleFor(command => command.Id).GreaterThan(0);
    }
}