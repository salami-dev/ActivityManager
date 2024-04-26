using ActivityManager.Application.ActivityTypes.Queries.GetActivityTypesWithPagination;
using ActivityManager.Application.Status.Queries.GetStatusWithPagination;
using ActivityManager.Application.Tags.Queries.GetTagsWithPagination;
using ActivityManager.Domain.Entities;

namespace ActivityManager.Application.Activities.Queries.GetActivity;

public class ActivityResponseDto
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; } = string.Empty;
    public string? Url { get; init; }
    public DateTimeOffset? StartDate { get; init; }
    public DateTimeOffset? EndDate { get; init; }
    public DateTimeOffset Created { get; init; }

    public StatusBriefResponseDto Status { get; init; } = null!;

    public ActivityTypeBriefResponseDto ActivityType { get; init; } = null!;

    public ICollection<TagBriefResponseDto> Tags { get; init; } = null!;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Activity, ActivityResponseDto>();
        }
    }
}
