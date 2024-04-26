using ActivityManager.Application.Common.Interfaces;

namespace ActivityManager.Application.Tags.Commands.UpdateTag;

public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
{
    private IApplicationDbContext _context;

    public UpdateTagCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        When(x => x.Name != null, () =>
        {
            RuleFor(y => y.Name)
                .NotEmpty()
                .MustAsync(
                    async (name, cancellationToken) => await UniqueName(name ?? "", cancellationToken)
                )
                .WithMessage("Another Tag with the same name exists");
        });

        RuleFor(x => x.Theme)
            .NotEmpty()
            .When(y => y.Name != null);
    }

    private async Task<bool> UniqueName(string name, CancellationToken cancellationToken)
    {
        var exists = await _context.Tags.AnyAsync(n => EF.Functions.ILike(n.Name, $"%{name}%"), cancellationToken);

        return !exists;
    }
}
