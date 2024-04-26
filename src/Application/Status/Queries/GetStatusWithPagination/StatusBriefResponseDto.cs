namespace ActivityManager.Application.Status.Queries.GetStatusWithPagination;

public class StatusBriefResponseDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Theme { get; init; }


    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Status, StatusBriefResponseDto>();
        }
    }
}
