using ActivityManager.Application.Common.Interfaces;

namespace ActivityManager.Application.ActivityTypes.Commands.DeleteActivityType;

public record DeleteActivityTypeCommand : IRequest<bool>
{
    public required int Id { get; init; }
}

public class DeleteActivityTypeCommandValidator : AbstractValidator<DeleteActivityTypeCommand>
{
    public DeleteActivityTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

public class DeleteActivityTypeCommandHandler : IRequestHandler<DeleteActivityTypeCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteActivityTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteActivityTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ActivityTypes.FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.ActivityTypes.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
