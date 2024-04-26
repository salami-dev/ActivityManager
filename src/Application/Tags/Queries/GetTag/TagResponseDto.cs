using ActivityManager.Application.Tasks.Queries.GetTask;
using ActivityManager.Domain.Entities;

namespace ActivityManager.Application.Tags.Queries.GetTag;

public class TagResponseDto
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Theme { get; init; }
    public DateTimeOffset Created { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Tag, TaskResponseDto>();
        }
    }
}
