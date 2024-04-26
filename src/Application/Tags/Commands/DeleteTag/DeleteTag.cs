using ActivityManager.Application.Common.Interfaces;

namespace ActivityManager.Application.Tags.Commands.DeleteTag;

public record DeleteTagCommand : IRequest<bool>
{
    public required int Id { get; init; }
}

public class DeleteTagCommandValidator : AbstractValidator<DeleteTagCommand>
{
    public DeleteTagCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteTagCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        // var entity = await _context.Tags.FindAsync(new object[] { request.Id }, cancellationToken);

        var entity = await _context.Tags
            .Include(x => x.Activities)
            .Include(x => x.Jobs)
            .Where(tag => tag.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);
        
        // clear relations first to avoid cascading
        // TODO: Set OnDelete Rule to SetNull instead of cascade
        
        entity.Activities.Clear();
        entity.Jobs.Clear();

        await _context.SaveChangesAsync(cancellationToken);

        _context.Tags.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
