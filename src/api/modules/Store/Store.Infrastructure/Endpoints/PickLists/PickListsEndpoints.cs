using Store.Infrastructure.Endpoints.PickLists.v1;

namespace Store.Infrastructure.Endpoints.PickLists;

/// <summary>
/// Endpoint configuration for Pick Lists module.
/// </summary>
public static class PickListsEndpoints
{
    /// <summary>
    /// Maps all Pick Lists endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapPickListsEndpoints(this IEndpointRouteBuilder app)
    {
        var pickListsGroup = app.MapGroup("/picklists")
            .WithTags("PickLists")
            .WithDescription("Endpoints for managing warehouse pick lists");

        // Version 1 endpoints
        pickListsGroup.MapCreatePickListEndpoint();
        pickListsGroup.MapAddPickListItemEndpoint();
        pickListsGroup.MapAssignPickListEndpoint();
        pickListsGroup.MapStartPickingEndpoint();
        pickListsGroup.MapCompletePickingEndpoint();
        pickListsGroup.MapDeletePickListEndpoint();
        pickListsGroup.MapGetPickListEndpoint();
        pickListsGroup.MapSearchPickListsEndpoint();

        return app;
    }
}
