using FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.GeneratedDocuments;

/// <summary>
/// Endpoint routes for managing generated documents.
/// </summary>
public class GeneratedDocumentsEndpoints() : CarterModule("humanresources")
{
    /// <summary>
    /// Maps all generated document endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/generated-documents").WithTags("generated-documents");

        group.MapPost("/", async (CreateGeneratedDocumentCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute("GetGeneratedDocument", new { id = response.Id }, response);
            })
            .WithName("CreateGeneratedDocumentEndpoint")
            .WithSummary("Creates a new generated document")
            .WithDescription("Generates a new document from a template")
            .Produces<CreateGeneratedDocumentResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetGeneratedDocumentRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetGeneratedDocumentEndpoint")
            .WithSummary("Gets generated document by ID")
            .WithDescription("Retrieves generated document details")
            .Produces<GeneratedDocumentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchGeneratedDocumentsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchGeneratedDocumentsEndpoint")
            .WithSummary("Searches generated documents")
            .WithDescription("Searches generated documents with pagination and filters")
            .Produces<PagedList<GeneratedDocumentResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateGeneratedDocumentCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateGeneratedDocumentEndpoint")
            .WithSummary("Updates a generated document")
            .WithDescription("Updates generated document status and information")
            .Produces<UpdateGeneratedDocumentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteGeneratedDocumentCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeleteGeneratedDocumentEndpoint")
            .WithSummary("Deletes a generated document")
            .WithDescription("Deletes a generated document")
            .Produces<DeleteGeneratedDocumentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);
    }
}

