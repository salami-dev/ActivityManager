using ActivityManager.Application.Common.Interfaces;

namespace ActivityManager.Application.Status.Commands.GetOrCreateStatus;

public record StatusData
{
    public required string Name { get; init; }
    public string Theme { get; init; } = string.Empty;
};

public record GetOrCreateStatusCommand : StatusData, IRequest<int>
{
}

public class GetOrCreateStatusCommandHandler : IRequestHandler<GetOrCreateStatusCommand, int>
{
    private readonly IApplicationDbContext _context;

    public GetOrCreateStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(GetOrCreateStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Status.Where(s => EF.Functions.ILike(s.Name, $"%{request.Name}%"))
            .FirstOrDefaultAsync(cancellationToken);

        if (entity != null) return entity.Id;

        var status = new Domain.Entities.Status()
        {
            Name = request.Name, Theme = request.Theme ?? Domain.Entities.Status.GenerateHexCode()
        };

        _context.Status.Add(status);

        await _context.SaveChangesAsync(cancellationToken);

        return status.Id;
    }
}
