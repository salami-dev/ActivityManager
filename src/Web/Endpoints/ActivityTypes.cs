using ActivityManager.Application.ActivityTypes.Commands.DeleteActivityType;
using ActivityManager.Application.ActivityTypes.Commands.UpdateActivityType;
using ActivityManager.Application.ActivityTypes.Queries.GetActivityTypesWithPagination;
using ActivityManager.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace ActivityManager.Web.Endpoints;

public class ActivityTypes : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetActivityTypesWithPagination)
            .MapPut(UpdateActivityType, "{id}")
            .MapDelete(DeleteActivityType, "{id}");
    }

    public async Task<PaginatedList<ActivityTypeBriefResponseDto>> GetActivityTypesWithPagination(ISender sender,
        [AsParameters] GetActivityTypesWithPaginationQuery query, CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }

    public async Task<IResult> UpdateActivityType(ISender sender, int id, [FromBody] UpdateActivityTypeCommand command,
        CancellationToken cancellationToken)
    {
        command.SetId(id);

        await sender.Send(command, cancellationToken);

        return Results.NoContent();
    }

    public async Task<IResult> DeleteActivityType(ISender sender, int id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteActivityTypeCommand() { Id = id }, cancellationToken);

        return Results.NoContent();
    }
}
