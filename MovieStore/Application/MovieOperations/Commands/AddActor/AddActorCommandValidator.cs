using FluentValidation;

namespace MovieStore.Application.MovieOperations.Commands.AddActor;

public class AddActorCommandValidator : AbstractValidator<AddActorCommand>
{
    public AddActorCommandValidator()
    {
        RuleFor(command => command.Id).GreaterThan(0);
        RuleFor(command => command.Model.ActorId).GreaterThan(0);
    }
}