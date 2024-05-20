using ActivityManager.Application.ActivityTypes.Commands.GetOrCreateActivityType;
using ActivityManager.Application.Common.Interfaces;
using ActivityManager.Application.Status.Commands.GetOrCreateStatus;
using ActivityManager.Application.Tags.Commands.BulkCreateTags;
using ActivityManager.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace ActivityManager.Application.Activities.Commands.CreateActivity;

public record CreateActivityCommand : IRequest<int>
{
    public required string Name { get; init; }
    public string Description { get; init; } = string.Empty;
    public string? Url { get; init; }
    public DateTimeOffset? StartDate { get; init; } = null;
    public DateTimeOffset? EndDate { get; init; } = null;

    public required StatusData Status { get; init; }
    public required ActivityTypeData ActivityType { get; init; }
    public List<CreateTagRequestDto> Tags { get; init; } = [];
}

public class CreateActivityCommandHandler : IRequestHandler<CreateActivityCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ISender _sender;

    public CreateActivityCommandHandler(IApplicationDbContext context, ISender sender)
    {
        _context = context;
        _sender = sender;
    }

    public async Task<int> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
    {
        var statusId =
            await _sender.Send(new GetOrCreateStatusCommand()
            {
                Name = request.Status.Name, Theme = request.Status.Theme
            });

        var activityTypeId =
            await _sender.Send(new GetOrCreateActivityTypeCommand() { Name = request.ActivityType.Name });

        List<int> tagIds = [];

        if (!request.Tags.IsNullOrEmpty())
        {
            tagIds = await _sender.Send(new BulkCreateTagsCommand() { Tags = request.Tags });
        }


        var entity = new Activity
        {
            Name = request.Name,
            Description = request.Description,
            Url = request.Url,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            ActivityTypeId = activityTypeId,
            StatusId = statusId
        };

        foreach (var tagId in tagIds)
        {
            var tag = new Tag() { Id = tagId };
            _context.Tags.Attach(tag);
            entity.Tags.Add(tag);
        }

        _context.Activities.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
