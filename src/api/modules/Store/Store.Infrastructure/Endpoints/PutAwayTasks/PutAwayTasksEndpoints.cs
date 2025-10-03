using Store.Infrastructure.Endpoints.PutAwayTasks.v1;

namespace Store.Infrastructure.Endpoints.PutAwayTasks;

public static class PutAwayTasksEndpoints
{
    public static IEndpointRouteBuilder MapPutAwayTasksEndpoints(this IEndpointRouteBuilder app)
    {
        var putAwayTasksGroup = app.MapGroup("/put-away-tasks").WithTags("put-away-tasks");

        putAwayTasksGroup.MapCreatePutAwayTaskEndpoint();
        putAwayTasksGroup.MapAddPutAwayTaskItemEndpoint();
        putAwayTasksGroup.MapAssignPutAwayTaskEndpoint();
        putAwayTasksGroup.MapStartPutAwayEndpoint();
        putAwayTasksGroup.MapCompletePutAwayEndpoint();
        putAwayTasksGroup.MapDeletePutAwayTaskEndpoint();
        putAwayTasksGroup.MapGetPutAwayTaskEndpoint();
        putAwayTasksGroup.MapSearchPutAwayTasksEndpoint();

        return app;
    }
}
