using ActivityManager.Application.Common.Models;
using ActivityManager.Application.Tags.Commands.DeleteTag;
using ActivityManager.Application.Tags.Commands.UpdateTag;
using ActivityManager.Application.Tags.Queries.GetTagsWithPagination;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;


namespace ActivityManager.Web.Endpoints;

public class Tags : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetTagsWithPagination)
            .MapPut(UpdateTag, "{id}")
            .MapDelete(DeleteTag, "{id}");
    }

    public async Task<PaginatedList<TagBriefResponseDto>> GetTagsWithPagination(ISender sender,
        [AsParameters] GetTagsWithPaginationQuery query, CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }

    public async Task<IResult> UpdateTag(ISender sender, int id, [FromBody] UpdateTagCommand command,
        CancellationToken cancellationToken)
    {
        command.SetId(id);

        await sender.Send(command, cancellationToken);

        return Results.NoContent();
    }

    public async Task<IResult> DeleteTag(ISender sender, int id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteTagCommand() { Id = id }, cancellationToken);

        return Results.NoContent();
    }
}
