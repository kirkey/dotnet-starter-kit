using FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDocuments;

/// <summary>
/// Endpoint configuration for EmployeeDocuments module.
/// </summary>
public class EmployeeDocumentsEndpoints() : CarterModule("humanresources")
{
    /// <summary>
    /// Maps all EmployeeDocuments endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/employee-documents").WithTags("employee-documents");

        group.MapPost("/", async (CreateEmployeeDocumentCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute("GetEmployeeDocument", new { id = response.Id }, response);
            })
            .WithName("CreateEmployeeDocumentEndpoint")
            .WithSummary("Creates a new employee document")
            .WithDescription("Creates a new employee document (contract, certification, license, etc.)")
            .Produces<CreateEmployeeDocumentResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetEmployeeDocumentRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetEmployeeDocumentEndpoint")
            .WithSummary("Gets employee document by ID")
            .WithDescription("Retrieves employee document details")
            .Produces<EmployeeDocumentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchEmployeeDocumentsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchEmployeeDocumentsEndpoint")
            .WithSummary("Searches employee documents")
            .WithDescription("Searches employee documents with pagination and filters")
            .Produces<PagedList<EmployeeDocumentResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateEmployeeDocumentCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateEmployeeDocumentEndpoint")
            .WithSummary("Updates an employee document")
            .WithDescription("Updates employee document information")
            .Produces<UpdateEmployeeDocumentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteEmployeeDocumentCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeleteEmployeeDocumentEndpoint")
            .WithSummary("Deletes an employee document")
            .WithDescription("Deletes an employee document record")
            .Produces<DeleteEmployeeDocumentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);
    }
}

