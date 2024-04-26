using ActivityManager.Domain.Entities;

namespace ActivityManager.Application.Tasks.Queries.GetTasksWithPagination;

public class TaskBriefResponseDto
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Content { get; init; }
    public required DateTimeOffset StartDate { get; init; }
    public required DateTimeOffset EndDate { get; init; }
    public DateTimeOffset Created { get; init; }

    // public StatusBriefResponseDto Status { get; init; } = null!;


    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Job, TaskBriefResponseDto>();
        }
    }
}
