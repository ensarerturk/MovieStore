using FluentValidation;

namespace MovieStore.Application.ActorOperations.Commands.CreateActor;

public class CreateActorCommandValidator : AbstractValidator<CreateActorCommand>
{
    public CreateActorCommandValidator()
    {
        RuleFor(command => command.Model.FirstName).NotEmpty().MinimumLength(1);
        RuleFor(command => command.Model.LastName).NotEmpty().MinimumLength(1);
    }
}