using FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.DocumentTemplates;

/// <summary>
/// Endpoint configuration for DocumentTemplates module.
/// </summary>
public class DocumentTemplatesEndpoints() : CarterModule("humanresources")
{
    /// <summary>
    /// Maps all DocumentTemplates endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/document-templates").WithTags("document-templates");

        group.MapPost("/", async (CreateDocumentTemplateCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute("GetDocumentTemplate", new { id = response.Id }, response);
            })
            .WithName("CreateDocumentTemplate")
            .WithSummary("Creates a new document template")
            .WithDescription("Creates a new document template for document generation")
            .Produces<CreateDocumentTemplateResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetDocumentTemplateRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetDocumentTemplate")
            .WithSummary("Gets document template by ID")
            .WithDescription("Retrieves document template details")
            .Produces<DocumentTemplateResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchDocumentTemplatesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchDocumentTemplates")
            .WithSummary("Searches document templates")
            .WithDescription("Searches document templates with pagination and filters")
            .Produces<PagedList<DocumentTemplateResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateDocumentTemplateCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateDocumentTemplate")
            .WithSummary("Updates a document template")
            .WithDescription("Updates document template information")
            .Produces<UpdateDocumentTemplateResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteDocumentTemplateCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeleteDocumentTemplate")
            .WithSummary("Deletes a document template")
            .WithDescription("Deletes a document template")
            .Produces<DeleteDocumentTemplateResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);
    }
}

