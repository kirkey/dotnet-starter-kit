using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.HumanResources.Application.Companies.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Companies.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.v1;

/// <summary>
/// Endpoint for searching companies.
/// </summary>
public static class SearchCompaniesEndpoint
{
    internal static RouteHandlerBuilder MapCompaniesSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchCompaniesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchCompaniesEndpoint))
            .WithSummary("Searches companies")
            .WithDescription("Searches companies with pagination and filters")
            .Produces<PagedList<CompanyResponse>>()
            .RequirePermission("Permissions.Companies.View")
            .MapToApiVersion(1);
    }
}

