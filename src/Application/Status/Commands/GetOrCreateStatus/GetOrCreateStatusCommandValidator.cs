namespace ActivityManager.Application.Status.Commands.GetOrCreateStatus;

public class GetOrCreateStatusCommandValidator : AbstractValidator<GetOrCreateStatusCommand>
{
    public GetOrCreateStatusCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(128);
    }
}
