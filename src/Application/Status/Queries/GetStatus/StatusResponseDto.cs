namespace ActivityManager.Application.Status.Queries.GetStatus;

public class StatusResponseDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Theme { get; init; }
    public DateTimeOffset Created { get; init; }


    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Status, StatusResponseDto>();
        }
    }
}
