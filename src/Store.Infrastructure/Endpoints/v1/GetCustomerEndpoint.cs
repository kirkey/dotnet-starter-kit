namespace Store.Infrastructure.Endpoints.v1;

public static class GetCustomerEndpoint
{
    internal static RouteHandlerBuilder MapGetCustomerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/customers/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new FSH.Starter.WebApi.Store.Application.Customers.Get.v1.GetCustomerQuery(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetCustomer")
        .WithSummary("Get customer by ID")
        .WithDescription("Retrieves a customer by their unique identifier")
        .MapToApiVersion(1);
    }
}

