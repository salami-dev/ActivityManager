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
                async (name, cancellationToken) => await UniqueName(name ?? "", cancellationToken)
            )
            .WithMessage("Another ActivityType with the same name exists");
    }

    private async Task<bool> UniqueName(string name, CancellationToken cancellationToken)
    {
        var exists =
            await _context.ActivityTypes.AnyAsync(n => EF.Functions.ILike(n.Name, $"%{name}%"), cancellationToken);

        return !exists;
    }
}
