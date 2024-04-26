namespace ActivityManager.Application.Activities.Queries.GetActivity;

public class GetActivityQueryValidator : AbstractValidator<GetActivityQuery>
{
    public GetActivityQueryValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
