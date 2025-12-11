using FSH.Starter.WebApi.Store.Application.Categories.Dashboard;

namespace Store.Infrastructure.Endpoints.Categories.v1;

public static class GetCategoryDashboardEndpoint
{
    internal static RouteHandlerBuilder MapGetCategoryDashboardEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id}/dashboard", async (DefaultIdType id, ISender sender) =>
        {
            var response = await sender.Send(new GetCategoryDashboardQuery(id));
            return Results.Ok(response);
        })
        .WithName(nameof(GetCategoryDashboardEndpoint))
        .WithSummary("Get comprehensive dashboard analytics for a specific category")
        .WithDescription("Returns detailed performance metrics including item statistics, inventory summary, sales trends, and alerts for the specified category and its subcategories.")
        .Produces<CategoryDashboardResponse>()
        .RequirePermission("Permissions.Categories.View")
        .MapToApiVersion(1)
        .WithOpenApi();
    }
}
