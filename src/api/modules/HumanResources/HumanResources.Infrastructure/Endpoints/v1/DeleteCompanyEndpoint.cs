using FSH.Starter.WebApi.HumanResources.Application.Companies.Delete.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.v1;

/// <summary>
/// Endpoint for deleting a company.
/// </summary>
public static class DeleteCompanyEndpoint
{
    internal static RouteHandlerBuilder MapCompanyDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteCompanyCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteCompanyEndpoint))
            .WithSummary("Deletes a company")
            .WithDescription("Deletes a company")
            .Produces<DeleteCompanyResponse>()
            .RequirePermission("Permissions.Companies.Delete")
            .MapToApiVersion(1);
    }
}

