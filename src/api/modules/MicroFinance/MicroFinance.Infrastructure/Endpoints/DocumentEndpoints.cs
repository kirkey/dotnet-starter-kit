using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.Documents.Archive.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Documents.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Documents.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Documents.MarkExpired.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Documents.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Documents.Update.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Documents.Verify.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class DocumentEndpoints : CarterModule
{

    private const string CreateDocument = "CreateDocument";
    private const string GetDocument = "GetDocument";
    private const string SearchDocuments = "SearchDocuments";
    private const string VerifyDocument = "VerifyDocument";
    private const string ArchiveDocument = "ArchiveDocument";
    private const string UpdateDocument = "UpdateDocument";
    private const string MarkExpiredDocument = "MarkExpiredDocument";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/documents").WithTags("Documents");

        group.MapPost("/", async (CreateDocumentCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/documents/{result.Id}", result);
        })
        .WithName(CreateDocument)
        .WithSummary("Create a new document record")
        .Produces<CreateDocumentResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetDocumentRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetDocument)
        .WithSummary("Get document by ID")
        .Produces<DocumentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id}/verify", async (DefaultIdType id, VerifyDocumentRequest request, ISender sender) =>
        {
            var result = await sender.Send(new VerifyDocumentCommand(id, request.VerifiedById));
            return Results.Ok(result);
        })
        .WithName(VerifyDocument)
        .WithSummary("Verify a document")
        .Produces<VerifyDocumentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchDocumentsCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SearchDocuments)
        .WithSummary("Search documents")
        .Produces<PagedList<DocumentSummaryResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateDocumentCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(UpdateDocument)
        .WithSummary("Update a document")
        .Produces<UpdateDocumentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id}/archive", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new ArchiveDocumentCommand(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(ArchiveDocument)
        .WithSummary("Archive a document")
        .Produces<ArchiveDocumentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id}/mark-expired", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new MarkExpiredDocumentCommand(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(MarkExpiredDocument)
        .WithSummary("Mark a document as expired")
        .Produces<MarkExpiredDocumentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public record VerifyDocumentRequest(DefaultIdType VerifiedById);
