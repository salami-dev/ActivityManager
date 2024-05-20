using ActivityManager.Application.Common.Interfaces;
using ActivityManager.Domain.Entities;

namespace ActivityManager.Application.Tags.Commands.CreateTag;

public record CreateTagCommand : IRequest<int>
{
    public required string Name { get; init; }
    public string Theme { get; init; } = "#000000";
}

public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateTagCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var entity = new Tag { Name = request.Name, Theme = request.Theme, };

        //entity.AddDomainEvent(new TodoItemCreatedEvent(entity));

        //_context.Tag.Add(entity);
        _context.Tags.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
