using ActivityManager.Application.Common.Interfaces;

namespace ActivityManager.Application.Tags.Commands.UpdateTag;

public record UpdateTagCommand : IRequest<bool>
{
    private int? _id;
    public int Id { get { return _id ?? 0; } }


    public string? Name { get; set; }
    public string? Theme { get; set; }

    public void SetId(int id)
    {
        _id = id;
    }
}

public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public UpdateTagCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Tags.FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Name = request.Name ?? entity.Name;
        entity.Theme = request.Theme ?? entity.Theme;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
