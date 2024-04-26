using ActivityManager.Application.Common.Interfaces;

namespace ActivityManager.Application.Activities.Queries.GetActivity;

public record GetActivityQuery : IRequest<ActivityResponseDto>
{
    public required int Id { get; init; }
}

public class GetActivityQueryHandler : IRequestHandler<GetActivityQuery, ActivityResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetActivityQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ActivityResponseDto> Handle(GetActivityQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Activities.AsNoTracking()
            .Include(a => a.Status)
            .Include(a => a.ActivityType)
            .Include(a => a.Tags)
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        // return entity;
        return _mapper.Map<ActivityResponseDto>(entity);
    }
}
