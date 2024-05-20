using ActivityManager.Application.Common.Interfaces;

namespace ActivityManager.Application.ActivityTypes.Commands.UpdateActivityType;

public class UpdateActivityTypeCommandValidator : AbstractValidator<UpdateActivityTypeCommand>
{
    private IApplicationDbContext _context;

    public UpdateActivityTypeCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MustAsync(
                async (model, name, cancellationToken) => await UniqueName(model, name ?? "", cancellationToken)
            )
            .WithMessage("Another ActivityType with the same name exists");
    }

    private async Task<bool> UniqueName(UpdateActivityTypeCommand model, string name,
        CancellationToken cancellationToken)
    {
        var exists = await _context.Tags.AnyAsync(n => n.Id != model.Id && n.Name.ToLower() == name.ToLower(),
            cancellationToken);

        return !exists;
    }
}
