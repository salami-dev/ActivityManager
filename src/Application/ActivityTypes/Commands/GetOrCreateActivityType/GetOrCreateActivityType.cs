using ActivityManager.Application.Common.Interfaces;

namespace ActivityManager.Application.ActivityTypes.Commands.GetOrCreateActivityType;

public record ActivityTypeData
{
    public required string Name { get; init; }
}

public record GetOrCreateActivityTypeCommand : ActivityTypeData, IRequest<int>
{
}

public class GetOrCreateActivityTypeCommandHandler : IRequestHandler<GetOrCreateActivityTypeCommand, int>
{
    private readonly IApplicationDbContext _context;

    public GetOrCreateActivityTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(GetOrCreateActivityTypeCommand request, CancellationToken cancellationToken)
    {
        var _e = await _context.ActivityTypes.Where(e => EF.Functions.ILike(e.Name, $"%{request.Name}%"))
            .FirstOrDefaultAsync();

        if (_e != null) return _e.Id;

        var entity = new Domain.Entities.ActivityType() { Name = request.Name };

        _context.ActivityTypes.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
