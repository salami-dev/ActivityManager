using System.Diagnostics;
using ActivityManager.Application.Common.Interfaces;
using ActivityManager.Application.Status.Commands.GetOrCreateStatus;
using ActivityManager.Application.Tags.Commands.BulkCreateTags;
using ActivityManager.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace ActivityManager.Application.Tasks.Commands.CreateTask;

public record CreateTaskCommand : IRequest<int>
{
    public required string Name { get; init; }
    public required string Content { get; init; }
    public required DateTimeOffset StartDate { get; init; }
    public required DateTimeOffset EndDate { get; init; }

    public int ActivityId { get; set; }

    public required StatusData Status { get; init; }
    public List<CreateTagRequestDto> Tags { get; init; } = [];
}

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ISender _sender;

    public CreateTaskCommandHandler(IApplicationDbContext context, ISender sender)
    {
        _context = context;
        _sender = sender;
    }

    public async Task<int> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var requestData = request;

        var statusId =
            await _sender.Send(new GetOrCreateStatusCommand()
            {
                Name = requestData.Status.Name, Theme = requestData.Status.Theme
            });

        List<int> tagIds = [];

        if (!requestData.Tags.IsNullOrEmpty())
        {
            tagIds = await _sender.Send(new BulkCreateTagsCommand() { Tags = request.Tags });
        }

        var entity = new Job
        {
            Name = requestData.Name,
            Content = requestData.Content,
            StartDate = requestData.StartDate,
            EndDate = requestData.EndDate,
            StatusId = statusId,
            ActivityId = requestData.ActivityId
        };

        foreach (var tagId in tagIds)
        {
            var tag = new Tag() { Id = tagId };
            _context.Tags.Attach(tag);
            entity.Tags.Add(tag);
        }

        _context.Jobs.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
