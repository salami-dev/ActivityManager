using ActivityManager.Application.Common.Interfaces;

namespace ActivityManager.Application.Status.Commands.DeleteStatus;

public record DeleteStatusCommand : IRequest<bool>
{
    public required int Id { get; init; }
}

public class DeleteStatusCommandHandler : IRequestHandler<DeleteStatusCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Status.FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.Status.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
