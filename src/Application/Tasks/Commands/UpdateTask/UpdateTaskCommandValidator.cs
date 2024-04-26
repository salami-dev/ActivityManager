using ActivityManager.Application.Common.Interfaces;

namespace ActivityManager.Application.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTaskCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .NotEmpty()
            .When(x => x.Name != null);
        RuleFor(x => x.Content)
            .NotEmpty()
            .When(x => x != null);
        RuleFor(x => x.Status)
            .NotEmpty()
            .When(x => x.Status != null);

        // Validate StartDate if it's provided
        When(x => x.StartDate != null, () =>
        {
            RuleFor(x => x.StartDate)
                .NotEmpty();

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .WithMessage("EndDate is required when StartDate is provided");

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("The end date must be on or after the start date");
        });

        // Validate EndDate if it's provided
        When(x => x.EndDate != null, () =>
        {
            RuleFor(x => x.EndDate)
                .NotEmpty();

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("StartDate is required when EndDate is provided");

            // This rule is already inside the StartDate block, so it's not strictly necessary here
            // unless you want to ensure it runs regardless of which date is entered first
            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("The end date must be on or after the start date");
        });

        RuleFor(x => x.ActivityId)
            .NotEmpty()
            .MustAsync(
                async (activityId, cancellationToken) => await ActivityExists(activityId ?? 0, cancellationToken))
            .WithMessage("Activity with the Specified ID does not exist.")
            .When(x => x.ActivityId != null);
    }

    private async Task<bool> ActivityExists(int activityId, CancellationToken cancellationToken)
    {
        return await _context.Activities.AnyAsync(a => a.Id == activityId, cancellationToken);
    }
}
