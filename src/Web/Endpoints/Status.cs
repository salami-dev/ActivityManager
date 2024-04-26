using ActivityManager.Application.Common.Models;
using ActivityManager.Application.Status.Commands.DeleteStatus;
using ActivityManager.Application.Status.Commands.UpdateStatus;
using ActivityManager.Application.Status.Queries.GetStatusWithPagination;
using Microsoft.AspNetCore.Mvc;

namespace ActivityManager.Web.Endpoints;

public class Status : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetStatusWithPagination)
            .MapPut(UpdateStatus, "{id}")
            .MapDelete(DeleteStatus, "{id}");
    }


    public async Task<PaginatedList<StatusBriefResponseDto>> GetStatusWithPagination(ISender sender,
        [AsParameters] GetStatusWithPaginationQuery query, CancellationToken cancellationToken)
    {
        return await sender.Send(query);
    }

    public async Task<IResult> UpdateStatus(ISender sender, int id, [FromBody] UpdateStatusCommand command,
        CancellationToken cancellationToken)
    {
        command.SetId(id);

        await sender.Send(command, cancellationToken);

        return Results.NoContent();
    }

    public async Task<IResult> DeleteStatus(ISender sender, int id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteStatusCommand() { Id = id }, cancellationToken);

        return Results.NoContent();
        
    }
}
