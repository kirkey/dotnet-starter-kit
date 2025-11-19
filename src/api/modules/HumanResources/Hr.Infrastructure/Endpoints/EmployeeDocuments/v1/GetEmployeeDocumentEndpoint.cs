using FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Get.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDocuments.v1;

public static class GetEmployeeDocumentEndpoint
{
    internal static RouteHandlerBuilder MapGetEmployeeDocumentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetEmployeeDocumentRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetEmployeeDocumentEndpoint))
            .WithSummary("Gets employee document by ID")
            .WithDescription("Retrieves employee document details")
            .Produces<EmployeeDocumentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);
    }
}

