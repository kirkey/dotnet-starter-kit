using FSH.Starter.WebApi.Store.Application.SerialNumbers.Get.v1;
using FSH.Starter.WebApi.Store.Application.SerialNumbers.Search.v1;

namespace Store.Infrastructure.Endpoints.SerialNumbers.v1;

public static class SearchSerialNumbersEndpoint
{
    internal static RouteHandlerBuilder MapSearchSerialNumbersEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchSerialNumbersCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchSerialNumbersEndpoint))
            .WithSummary("Search serial numbers")
            .WithDescription("Searches for serial numbers with pagination and filtering by serial value, item, warehouse, status, receipt date, warranty status, and external reference.")
            .Produces<PagedList<SerialNumberResponse>>()
            .RequirePermission("Permissions.Store.View")
            .MapToApiVersion(1);
    }
}
