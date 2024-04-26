using ActivityManager.Application.Common.Interfaces;
using ActivityManager.Application.Status.Commands.GetOrCreateStatus;
using ActivityManager.Application.Tags.Commands.BulkCreateTags;
using ActivityManager.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace ActivityManager.Application.Tasks.Commands.UpdateTask;

public record UpdateTaskCommand : IRequest<bool>
{
    private int? _id;
    public int Id { get { return _id ?? 0; } }
    public string? Name { get; init; }
    public string? Content { get; init; }
    public int? ActivityId { get; init; } = null;
    public DateTimeOffset? StartDate { get; init; }
    public DateTimeOffset? EndDate { get; init; }

    public StatusData? Status { get; init; }

    public List<CreateTagRequestDto>? Tags { get; init; }

    public void SetId(int id)
    {
        _id = id;
    }
}

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ISender _sender;

    public UpdateTaskCommandHandler(IApplicationDbContext context, ISender sender)
    {
        _context = context;
        _sender = sender;
    }

    public async Task<bool> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Jobs.FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Name = request.Name ?? entity.Name;
        entity.Content = request.Content ?? entity.Content;
        entity.ActivityId = request.ActivityId ?? entity.ActivityId;
        entity.StartDate = request.StartDate ?? entity.StartDate;
        entity.EndDate = request.EndDate ?? entity.EndDate;

        if (request.Status != null)
        {
            var statusId =
                await _sender.Send(new GetOrCreateStatusCommand()
                {
                    Name = request.Status.Name, Theme = request.Status.Theme
                });

            entity.StatusId = statusId;
        }

        if (request.Tags != null)
        {
            var tagIds = !request.Tags.IsNullOrEmpty()
                ? await _sender.Send(new BulkCreateTagsCommand() { Tags = request.Tags })
                : [];

            await _context.Entry(entity).Collection(x => x.Tags).LoadAsync(cancellationToken);

            entity.Tags.Clear();

            await _context.SaveChangesAsync(cancellationToken);

            foreach (var tagId in tagIds)
            {
                var tag = _context.Tags.Local.FirstOrDefault(t => t.Id == tagId);
                if (tag == null)
                {
                    tag = new Tag() { Id = tagId };
                    _context.Tags.Attach(tag);
                }

                entity.Tags.Add(tag);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
