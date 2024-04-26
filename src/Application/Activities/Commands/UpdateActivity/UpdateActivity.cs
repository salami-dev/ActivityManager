using ActivityManager.Application.ActivityTypes.Commands.GetOrCreateActivityType;
using ActivityManager.Application.Common.Interfaces;
using ActivityManager.Application.Status.Commands.GetOrCreateStatus;
using ActivityManager.Application.Tags.Commands.BulkCreateTags;
using ActivityManager.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace ActivityManager.Application.Activities.Commands.UpdateActivity;

public class UpdateActivityCommand : IRequest<bool>
{
    private int? _id;
    public int Id { get { return _id ?? 0; } }

    public string? Name { get; init; }

    public string? Description { get; init; }

    public string? Url { get; init; }

    public DateTimeOffset? StartDate { get; init; }

    public DateTimeOffset? EndDate { get; init; }

    public StatusData? Status { get; init; }

    public ActivityTypeData? ActivityType { get; init; }

    public List<CreateTagRequestDto>? Tags { get; init; }

    public void SetId(int id)
    {
        _id = id;
    }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UpdateActivityCommand, Activity>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}

public class UpdateActivityCommandHandler : IRequestHandler<UpdateActivityCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public UpdateActivityCommandHandler(IApplicationDbContext context, IMapper mapper, ISender sender)
    {
        _context = context;
        _mapper = mapper;
        _sender = sender;
    }

    public async Task<bool> Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Activities.FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Name = request.Name ?? entity.Name;
        entity.Description = request.Description ?? entity.Description;
        entity.Url = request.Url ?? entity.Url;
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


        if (request.ActivityType != null)
        {
            var activityTypeId =
                await _sender.Send(new GetOrCreateActivityTypeCommand() { Name = request.ActivityType.Name });

            entity.ActivityTypeId = activityTypeId;
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

        // _mapper.Map(request, entity);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
