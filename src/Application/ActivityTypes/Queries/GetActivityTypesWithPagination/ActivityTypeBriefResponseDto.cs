namespace ActivityManager.Application.ActivityTypes.Queries.GetActivityTypesWithPagination;

public class ActivityTypeBriefResponseDto
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;


    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.ActivityType, ActivityTypeBriefResponseDto>();
        }
    }
}
