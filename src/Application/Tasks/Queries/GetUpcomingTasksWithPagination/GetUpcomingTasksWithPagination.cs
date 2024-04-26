using ActivityManager.Application.Common.Interfaces;
using ActivityManager.Application.Tasks.Queries.GetTask;

namespace ActivityManager.Application.Tasks.Queries.GetUpcomingTasksWithPagination;

public record GetUpcomingTasksWithPaginationQuery : IRequest<TaskResponseDto>
{
}

public class GetUpcomingTasksWithPaginationQueryValidator : AbstractValidator<GetUpcomingTasksWithPaginationQuery>
{
    public GetUpcomingTasksWithPaginationQueryValidator()
    {
    }
}

public class GetUpcomingTasksWithPaginationQueryHandler : IRequestHandler<GetUpcomingTasksWithPaginationQuery, TaskResponseDto>
{
    private readonly IApplicationDbContext _context;

    public GetUpcomingTasksWithPaginationQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TaskResponseDto> Handle(GetUpcomingTasksWithPaginationQuery request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        
        throw new NotImplementedException();
    }
}
