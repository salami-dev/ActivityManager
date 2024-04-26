using ActivityManager.Application.Activities.Queries.GetActivity;

namespace ActivityManager.Application.ActivityTypes.Queries.GetActivityType;

public class ActivityTypeResponseDto
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public DateTimeOffset Created { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.ActivityType, ActivityResponseDto>();
        }
    }
}
