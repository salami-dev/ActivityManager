using ActivityManager.Application.Common.Interfaces;
using ActivityManager.Application.Common.Mappings;
using ActivityManager.Application.Common.Models;
using ActivityManager.Domain.Entities;

namespace ActivityManager.Application.ActivityTypes.Queries.GetActivityTypesWithPagination;

public record GetActivityTypesWithPaginationQuery : IRequest<PaginatedList<ActivityTypeBriefResponseDto>>
{
    public string? Search { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetActivityTypesWithPaginationQueryValidator : AbstractValidator<GetActivityTypesWithPaginationQuery>
{
    public GetActivityTypesWithPaginationQueryValidator()
    {
        RuleFor(x => x.Search)
            .MinimumLength(1);
    }
}

public class
    GetActivityTypesWithPaginationQueryHandler : IRequestHandler<GetActivityTypesWithPaginationQuery,
    PaginatedList<ActivityTypeBriefResponseDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetActivityTypesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ActivityTypeBriefResponseDto>> Handle(GetActivityTypesWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var searchFilter = request.Search;

        IQueryable<ActivityType> query = _context.ActivityTypes;

        if (!string.IsNullOrEmpty(searchFilter))
        {
            query = query.Where(x => EF.Functions.ILike(x.Name, $"%{searchFilter}%"));
        }

        return await query.OrderBy(x => x.Name)
            .ProjectTo<ActivityTypeBriefResponseDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
