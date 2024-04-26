using ActivityManager.Domain.Entities;

namespace ActivityManager.Application.Tags.Queries.GetTagsWithPagination;

public class TagBriefResponseDto
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Theme { get; init; }
    public DateTimeOffset Created { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Tag, TagBriefResponseDto>();
        }
    }
}
