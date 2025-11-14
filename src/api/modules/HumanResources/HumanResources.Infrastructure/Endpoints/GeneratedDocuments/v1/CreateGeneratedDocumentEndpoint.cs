using FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Create.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.GeneratedDocuments.v1;

public static class CreateGeneratedDocumentEndpoint
{
    internal static RouteHandlerBuilder MapCreateGeneratedDocumentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateGeneratedDocumentCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GetGeneratedDocumentEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreateGeneratedDocumentEndpoint))
            .WithSummary("Creates a new generated document")
            .WithDescription("Generates a new document from a template")
            .Produces<CreateGeneratedDocumentResponse>(StatusCodes.Status201Created)
            .RequirePermission("Permissions.Documents.Manage")
            .MapToApiVersion(1);
    }
}

