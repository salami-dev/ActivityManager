using ActivityManager.Application.Common.Interfaces;

namespace ActivityManager.Application.Tasks.Commands.CreateTask;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateTaskCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .NotEmpty();
        RuleFor(x => x.Content)
            .NotEmpty();
        RuleFor(x => x.Status)
            .NotEmpty();
        RuleFor(x => x.EndDate)
            .NotEmpty();
        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("The end date must be on or after the start date.");
        RuleFor(x => x.ActivityId)
            .NotEmpty()
            .MustAsync(async (activityId, cancellationToken) => await ActivityExists(activityId, cancellationToken))
            .WithMessage("Activity with the Specified ID does not exist.");
    }

    private async Task<bool> ActivityExists(int activityId, CancellationToken cancellationToken)
    {
        return await _context.Activities.AnyAsync(a => a.Id == activityId, cancellationToken);
    }
}
