using ActivityManager.Application.Common.Interfaces;

namespace ActivityManager.Application.Tasks.Queries.GetTask;

public record GetTaskQuery : IRequest<TaskResponseDto>
{
    public required int Id { get; init; }
}

public class GetTaskQueryValidator : AbstractValidator<GetTaskQuery>
{
    public GetTaskQueryValidator()
    {
    }
}

public class GetTaskQueryHandler : IRequestHandler<GetTaskQuery, TaskResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTaskQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TaskResponseDto> Handle(GetTaskQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Jobs
            .AsNoTracking()
            .Include(j => j.Status)
            .Include(j => j.Activity)
            .Include(j => j.Tags)
            .FirstOrDefaultAsync(j => j.Id == request.Id, cancellationToken);


        Guard.Against.NotFound(request.Id, entity);

        return _mapper.Map<TaskResponseDto>(entity);
    }
}
