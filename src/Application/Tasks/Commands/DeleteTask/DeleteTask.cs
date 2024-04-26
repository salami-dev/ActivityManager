using ActivityManager.Application.Common.Interfaces;

namespace ActivityManager.Application.Tasks.Commands.DeleteTask;

public record DeleteTaskCommand : IRequest<bool>
{
    public required int Id { get; init; }
}

public class DeleteTaskCommandValidator : AbstractValidator<DeleteTaskCommand>
{
    public DeleteTaskCommandValidator()
    {
    }
}

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Jobs.FindAsync(new { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.Jobs.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
