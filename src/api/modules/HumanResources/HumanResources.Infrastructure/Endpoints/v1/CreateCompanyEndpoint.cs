using FSH.Starter.WebApi.HumanResources.Application.Companies.Create.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.v1;

/// <summary>
/// Endpoint for creating a company.
/// </summary>
public static class CreateCompanyEndpoint
{
    internal static RouteHandlerBuilder MapCompanyCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateCompanyCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateCompanyEndpoint))
            .WithSummary("Creates a new company")
            .WithDescription("Creates a new company in the system for multi-entity support")
            .Produces<CreateCompanyResponse>()
            .RequirePermission("Permissions.Companies.Create")
            .MapToApiVersion(1);
    }
}

