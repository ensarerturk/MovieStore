using FluentValidation;

namespace MovieStore.Application.DirectorOperations.Commands.UpdateDirector;

public class UpdateDirectorCommandValidator : AbstractValidator<UpdateDirectorCommand>
{
    public UpdateDirectorCommandValidator()
    {
        RuleFor(command => command.Id).GreaterThan(0);
        RuleFor(command => command.Model.FirstName).MinimumLength(1)
            .When(command => command.Model.FirstName != string.Empty);
        RuleFor(command => command.Model.LastName).MinimumLength(1)
            .When(command => command.Model.LastName != string.Empty);
    }
}