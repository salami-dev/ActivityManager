using ActivityManager.Application.Common.Interfaces;
using ActivityManager.Application.Common.Mappings;
using ActivityManager.Application.Common.Models;

namespace ActivityManager.Application.Status.Queries.GetStatusWithPagination;

public record GetStatusWithPaginationQuery : IRequest<PaginatedList<StatusBriefResponseDto>>
{
    public string? Search { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetStatusWithPaginationQueryValidator : AbstractValidator<GetStatusWithPaginationQuery>
{
    public GetStatusWithPaginationQueryValidator()
    {
        // RuleFor(x => x.Search)
        //     .Empty();
    }
}

public class
    GetStatusWithPaginationQueryHandler : IRequestHandler<GetStatusWithPaginationQuery,
    PaginatedList<StatusBriefResponseDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetStatusWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<StatusBriefResponseDto>> Handle(GetStatusWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var searchFilter = request.Search;

        IQueryable<Domain.Entities.Status> query = _context.Status;

        if (!string.IsNullOrEmpty(searchFilter))
        {
            query = query.Where(x => EF.Functions.ILike(x.Name, $"%{searchFilter}%"));
        }

        return await query.OrderBy(x => x.Name)
            .ProjectTo<StatusBriefResponseDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
