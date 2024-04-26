using ActivityManager.Application.Common.Interfaces;
using ActivityManager.Application.Common.Mappings;
using ActivityManager.Application.Common.Models;
using ActivityManager.Domain.Entities;

namespace ActivityManager.Application.Tasks.Queries.GetTasksWithPagination;

public record GetTasksWithPaginationQuery : IRequest<PaginatedList<TaskBriefResponseDto>>
{
    public string? Search { get; init; }
    public int? ActivityId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class
    GetTasksWithPaginationQueryHandler : IRequestHandler<GetTasksWithPaginationQuery,
    PaginatedList<TaskBriefResponseDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTasksWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TaskBriefResponseDto>> Handle(GetTasksWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var searchFilter = request.Search;

        IQueryable<Job> query = _context.Jobs;

        if (!string.IsNullOrEmpty(searchFilter))
        {
            query = query.Where(x => EF.Functions.ILike(x.Name, $"%{searchFilter}%"));
        }

        if (request.ActivityId.HasValue)
        {
            query = query.Where(x => x.ActivityId == request.ActivityId.Value);
        }

        return await query.OrderBy(x => x.Name)
            .ProjectTo<TaskBriefResponseDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
