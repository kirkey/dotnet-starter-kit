using FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Create.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.DocumentTemplates.v1;

public static class CreateDocumentTemplateEndpoint
{
    internal static RouteHandlerBuilder MapCreateDocumentTemplateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateDocumentTemplateCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GetDocumentTemplateEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreateDocumentTemplateEndpoint))
            .WithSummary("Creates a new document template")
            .WithDescription("Creates a new document template for document generation")
            .Produces<CreateDocumentTemplateResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);
    }
}
