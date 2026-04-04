using FluentValidation;

namespace TalkPulse.Api.Features.Sessions.CreateSession;

public sealed class CreateSessionValidator : AbstractValidator<CreateSessionCommand>
{
    public CreateSessionValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Session title is required.")
            .MaximumLength(100)
            .WithMessage("Session title must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Session description must not exceed 1000 characters.");
    }
}