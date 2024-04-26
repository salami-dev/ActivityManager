using ActivityManager.Application.Common.Interfaces;

namespace ActivityManager.Application.Activities.Commands.DeleteActivity;

public record DeleteActivityCommand : IRequest
{
    public int Id;
}

public class DeleteActivityCommandHandler : IRequestHandler<DeleteActivityCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteActivityCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Activities.FindAsync(new { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.Activities.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
