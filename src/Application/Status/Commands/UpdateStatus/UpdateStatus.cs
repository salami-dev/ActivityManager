using ActivityManager.Application.Common.Interfaces;

namespace ActivityManager.Application.Status.Commands.UpdateStatus;

public record UpdateStatusCommand : IRequest<bool>
{
    private int? _id;
    public int Id { get { return _id ?? 0; } }

    public string? Name { get; init; }
    public string? Theme { get; init; }

    public void SetId(int id)
    {
        _id = id;
    }
}

public class UpdateStatusCommandHandler : IRequestHandler<UpdateStatusCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public UpdateStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Status.FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Name = request.Name ?? entity.Name;
        entity.Theme = request.Theme ?? entity.Theme;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
