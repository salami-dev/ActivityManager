using ActivityManager.Application.Activities.Commands.CreateActivity;
using ActivityManager.Application.Activities.Commands.DeleteActivity;
using ActivityManager.Application.Activities.Commands.UpdateActivity;
using ActivityManager.Application.Activities.Queries.GetActivitiesWithPagination;
using ActivityManager.Application.Activities.Queries.GetActivity;
using ActivityManager.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace ActivityManager.Web.Endpoints;

public class Activities : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetActivitiesWithPagination)
            .MapGet(GetActivity, "{id}")
            .MapPost(CreateActivity)
            .MapPut(UpdateActivity, "{id}")
            .MapDelete(DeleteActivity, "{id}");
    }

    public async Task<int> CreateActivity(ISender sender, CreateActivityCommand command,
        CancellationToken cancellationToken)
    {
        return await sender.Send(command, cancellationToken);
    }

    public async Task<PaginatedList<ActivityBriefDto>> GetActivitiesWithPagination(ISender sender,
        [AsParameters] GetActivitiesWithPaginationQuery query, CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }

    public async Task<ActivityResponseDto> GetActivity(ISender sender, int id,
        CancellationToken cancellationToken)
    {
        var query = new GetActivityQuery() { Id = id };
        return await sender.Send(query, cancellationToken);
    }

    public async Task<IResult> DeleteActivity(ISender sender, int id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteActivityCommand() { Id = id }, cancellationToken);

        return Results.NoContent();
    }

    public async Task<IResult> UpdateActivity(ISender sender, int id, [FromBody] UpdateActivityCommand command,
        CancellationToken cancellationToken)
    {
        command.SetId(id);

        await sender.Send(command, cancellationToken);

        return Results.NoContent();
    }
}
