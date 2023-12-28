using FluentValidation;

namespace MovieStore.Application.ActorOperations.Commands.UpdateActor;

public class UpdateActorCommandValidator : AbstractValidator<UpdateActorCommand>
{
    public UpdateActorCommandValidator()
    {
        RuleFor(command => command.Id).GreaterThan(0);
        RuleFor(command => command.Model.FirstName).MinimumLength(1)
            .When(command => command.Model.FirstName != string.Empty);
        RuleFor(command => command.Model.LastName).MinimumLength(1)
            .When(command => command.Model.LastName != string.Empty);
    }
}