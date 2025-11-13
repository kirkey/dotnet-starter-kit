using FSH.Starter.WebApi.HumanResources.Application.Companies.Update.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.v1;

/// <summary>
/// Endpoint for updating a company.
/// </summary>
public static class UpdateCompanyEndpoint
{
    internal static RouteHandlerBuilder MapCompanyUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, UpdateCompanyCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateCompanyEndpoint))
            .WithSummary("Updates a company")
            .WithDescription("Updates company information")
            .Produces<UpdateCompanyResponse>()
            .RequirePermission("Permissions.Companies.Update")
            .MapToApiVersion(1);
    }
}

