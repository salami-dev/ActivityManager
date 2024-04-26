namespace ActivityManager.Application.ActivityTypes.Commands.GetOrCreateActivityType;

public class GetOrCreateActivityTypeCommandValidator : AbstractValidator<GetOrCreateActivityTypeCommand>
{
    public GetOrCreateActivityTypeCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(128);
    }
}
