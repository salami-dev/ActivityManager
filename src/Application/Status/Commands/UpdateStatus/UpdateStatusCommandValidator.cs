using ActivityManager.Application.Common.Interfaces;

namespace ActivityManager.Application.Status.Commands.UpdateStatus;

public class UpdateStatusCommandValidator : AbstractValidator<UpdateStatusCommand>
{
    private IApplicationDbContext _context;

    public UpdateStatusCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        When(x => x.Name != null, () =>
        {
            RuleFor(y => y.Name)
                .NotEmpty()
                .MustAsync(
                    async (name, cancellationToken) => await UniqueName(name ?? "", cancellationToken)
                )
                .WithMessage("Another Status with the same name exists");
        });

        RuleFor(x => x.Theme)
            .NotEmpty()
            .When(y => y.Name != null);
    }

    private async Task<bool> UniqueName(string name, CancellationToken cancellationToken)
    {
        var exists = await _context.Status.AnyAsync(n => EF.Functions.ILike(n.Name, $"%{name}%"), cancellationToken);

        return !exists;
    }
}
