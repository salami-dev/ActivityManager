using ActivityManager.Application.Common.Models;
using ActivityManager.Application.Tasks.Commands.CreateTask;
using ActivityManager.Application.Tasks.Commands.DeleteTask;
using ActivityManager.Application.Tasks.Commands.UpdateTask;
using ActivityManager.Application.Tasks.Queries.GetTask;
using ActivityManager.Application.Tasks.Queries.GetTasksWithPagination;
using Microsoft.AspNetCore.Mvc;

namespace ActivityManager.Web.Endpoints;

public class Tasks : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetTasksWithPagination)
            .MapGet(GetTask, "{id}")
            .MapPost(CreateTask)
            .MapPut(UpdateTask, "{id}")
            .MapDelete(DeleteTask, "{id}");
    }

    public async Task<int> CreateTask(ISender sender, CreateTaskCommand command, CancellationToken cancellationToken)
    {
        return await sender.Send(command, cancellationToken);
    }

    public async Task<PaginatedList<TaskBriefResponseDto>> GetTasksWithPagination(ISender sender,
        [AsParameters] GetTasksWithPaginationQuery query, CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }

    public async Task<TaskResponseDto> GetTask(ISender sender, int id, CancellationToken cancellationToken)
    {
        return await sender.Send(new GetTaskQuery() { Id = id });
    }

    public async Task<IResult> DeleteTask(ISender sender, int id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteTaskCommand() { Id = id });

        return Results.NoContent();
    }

    public async Task<IResult> UpdateTask(ISender sender, int id, [FromBody] UpdateTaskCommand command,
        CancellationToken cancellationToken)
    {
        command.SetId(id);

        await sender.Send(command, cancellationToken);

        return Results.NoContent();
    }
}
