using FSH.Starter.WebApi.HumanResources.Application.Companies.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.v1;

/// <summary>
/// Endpoint for getting a company by ID.
/// </summary>
public static class GetCompanyEndpoint
{
    internal static RouteHandlerBuilder MapCompanyGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetCompanyRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetCompanyEndpoint))
            .WithSummary("Gets company by ID")
            .WithDescription("Retrieves company details by ID")
            .Produces<CompanyResponse>()
            .RequirePermission("Permissions.Companies.View")
            .MapToApiVersion(1);
    }
}

