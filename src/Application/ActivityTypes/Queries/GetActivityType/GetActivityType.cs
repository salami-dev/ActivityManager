using ActivityManager.Application.Common.Interfaces;

namespace ActivityManager.Application.ActivityTypes.Queries.GetActivityType;

public record GetActivityTypeQuery : IRequest<ActivityTypeResponseDto>
{
    public int Id;
}

public class GetActivityTypeQueryHandler : IRequestHandler<GetActivityTypeQuery, ActivityTypeResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetActivityTypeQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ActivityTypeResponseDto> Handle(GetActivityTypeQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.ActivityTypes.FindAsync(new { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        return _mapper.Map<ActivityTypeResponseDto>(entity);
    }
}
