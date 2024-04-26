using ActivityManager.Application.Common.Interfaces;

namespace ActivityManager.Application.Tasks.Queries.GetTasksWithPagination;

public class GetTasksWithPaginationQueryValidator : AbstractValidator<GetTasksWithPaginationQuery>
{
    private readonly IApplicationDbContext _context;

    public GetTasksWithPaginationQueryValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.ActivityId)
            .NotEmpty().WithMessage("Activity ID must not be empty.")
            .MustAsync(async (activityId, cancellationToken) =>
                activityId.HasValue && await ActivityExists(activityId.Value, cancellationToken))
            .WithMessage("Activity with the Specified ID does not exist.")
            .When(a => a.ActivityId.HasValue);
    }

    private async Task<bool> ActivityExists(int activityId, CancellationToken cancellationToken)
    {
        return await _context.Activities.AnyAsync(a => a.Id == activityId, cancellationToken);
    }
}
