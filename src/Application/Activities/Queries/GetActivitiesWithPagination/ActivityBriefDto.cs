using ActivityManager.Domain.Entities;

namespace ActivityManager.Application.Activities.Queries.GetActivitiesWithPagination;

public class ActivityBriefDto
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }

    public string? Url { get; init; }
    
    public DateTimeOffset? StartDate { get; init; } = null;
    public DateTimeOffset? EndDate { get; init; }
    public DateTimeOffset? Created { get; init; }


    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Activity, ActivityBriefDto>();
        }
    }
}
