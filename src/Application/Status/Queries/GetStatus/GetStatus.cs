using ActivityManager.Application.Common.Interfaces;

namespace ActivityManager.Application.Status.Queries.GetStatus;

public record GetStatusQuery : IRequest<StatusResponseDto>
{
}

public class GetStatusQueryValidator : AbstractValidator<GetStatusQuery>
{
    public GetStatusQueryValidator()
    {
    }
}

public class GetStatusQueryHandler : IRequestHandler<GetStatusQuery, StatusResponseDto>
{
    private readonly IApplicationDbContext _context;

    public GetStatusQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StatusResponseDto> Handle(GetStatusQuery request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }
}
