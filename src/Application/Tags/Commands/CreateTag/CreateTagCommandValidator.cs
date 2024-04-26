namespace ActivityManager.Application.Tags.Commands.CreateTag;
public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator()
    {
        RuleFor(v => v.Name).NotEmpty();
        RuleFor(v => v.Theme).NotEmpty();
    }
}
