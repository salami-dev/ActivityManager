using ActivityManager.Domain.Entities;

namespace ActivityManager.Application.Tags.Commands.BulkCreateTags;

public class CreateTagRequestDto
{
    public required string Name { get; init; }
    public string Theme { get; init; } = string.Empty;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CreateTagRequestDto, Tag>();
        }
    }
}
