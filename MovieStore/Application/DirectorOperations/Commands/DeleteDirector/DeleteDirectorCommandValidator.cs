using FluentValidation;

namespace MovieStore.Application.DirectorOperations.Commands.DeleteDirector;

public class DeleteDirectorCommandValidator : AbstractValidator<DeleteDirectorCommand>
{
    public DeleteDirectorCommandValidator()
    {
        RuleFor(command => command.Id).GreaterThan(0);
    }
}