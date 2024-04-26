using ActivityManager.Application.Common.Interfaces;

namespace ActivityManager.Application.Tags.Queries.GetTag;

public record GetTagQuery : IRequest<TagResponseDto>
{
}

public class GetTagQueryHandler : IRequestHandler<GetTagQuery, TagResponseDto>
{
    private readonly IApplicationDbContext _context;

    public GetTagQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TagResponseDto> Handle(GetTagQuery request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }
}
