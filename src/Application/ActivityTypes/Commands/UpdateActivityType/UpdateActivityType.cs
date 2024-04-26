using ActivityManager.Application.Common.Interfaces;

namespace ActivityManager.Application.ActivityTypes.Commands.UpdateActivityType;

public record UpdateActivityTypeCommand : IRequest<bool>
{
    private int? _id;
    public int Id { get { return _id ?? 0; } }

    public string? Name { get; init; }

    public void SetId(int id)
    {
        _id = id;
    }
}

public class UpdateActivityTypeCommandHandler : IRequestHandler<UpdateActivityTypeCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public UpdateActivityTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateActivityTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ActivityTypes.FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Name = request.Name ?? entity.Name;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
