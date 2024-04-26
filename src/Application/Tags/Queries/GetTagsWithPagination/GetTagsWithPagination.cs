using ActivityManager.Application.Common.Interfaces;
using ActivityManager.Application.Common.Mappings;
using ActivityManager.Application.Common.Models;
using ActivityManager.Domain.Entities;

namespace ActivityManager.Application.Tags.Queries.GetTagsWithPagination;

public record GetTagsWithPaginationQuery : IRequest<PaginatedList<TagBriefResponseDto>>
{
    public string? Search { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetTagsWithPaginationQueryValidator : AbstractValidator<GetTagsWithPaginationQuery>
{
    public GetTagsWithPaginationQueryValidator()
    {
    }
}

public class
    GetTagsWithPaginationQueryHandler : IRequestHandler<GetTagsWithPaginationQuery, PaginatedList<TagBriefResponseDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTagsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TagBriefResponseDto>> Handle(GetTagsWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var searchFilter = request.Search;

        IQueryable<Tag> query = _context.Tags;

        if (!string.IsNullOrEmpty(searchFilter))
        {
            query = query.Where(x => EF.Functions.ILike(x.Name, $"%{searchFilter}%"));
        }

        return await query.OrderBy(x => x.Name)
            .ProjectTo<TagBriefResponseDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
