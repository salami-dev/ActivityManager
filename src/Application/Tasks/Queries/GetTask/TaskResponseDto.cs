using ActivityManager.Application.Activities.Queries.GetActivitiesWithPagination;
using ActivityManager.Application.Status.Queries.GetStatusWithPagination;
using ActivityManager.Application.Tags.Queries.GetTagsWithPagination;
using ActivityManager.Domain.Entities;

namespace ActivityManager.Application.Tasks.Queries.GetTask;

public class TaskResponseDto
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Content { get; init; }
    public required DateTimeOffset StartDate { get; init; }
    public required DateTimeOffset EndDate { get; init; }
    public DateTimeOffset Created { get; init; }

    public StatusBriefResponseDto Status { get; init; } = null!;

    public ActivityBriefDto Activity { get; init; } = null!;

    public ICollection<TagBriefResponseDto> Tags { get; init; } = null!;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Job, TaskResponseDto>();
        }
    }
}
