using ActivityManager.Application.Common.Interfaces;
using ActivityManager.Application.Common.Mappings;
using ActivityManager.Application.Common.Models;
using ActivityManager.Domain.Entities;

namespace ActivityManager.Application.Activities.Queries.GetActivitiesWithPagination;

public record GetActivitiesWithPaginationQuery : IRequest<PaginatedList<ActivityBriefDto>>
{
    public string? Search { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class
    GetActivitiesWithPaginationQueryHandler : IRequestHandler<GetActivitiesWithPaginationQuery,
    PaginatedList<ActivityBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetActivitiesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ActivityBriefDto>> Handle(GetActivitiesWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var searchFilter = request.Search;

        IQueryable<Activity> query = _context.Activities;

        if (!string.IsNullOrEmpty(searchFilter))
        {
            query = query.Where(x => EF.Functions.ILike(x.Name, $"%{searchFilter}%"));
        }

        return await query
            .AsNoTracking()
            .OrderByDescending(x => x.Created)
            .ProjectTo<ActivityBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
