using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.Documents.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Documents.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Documents.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Documents.Verify.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class DocumentEndpoints : CarterModule
{

    private const string CreateDocument = "CreateDocument";
    private const string GetDocument = "GetDocument";
    private const string SearchDocuments = "SearchDocuments";
    private const string VerifyDocument = "VerifyDocument";

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

        group.MapPost("/{id:guid}/verify", async (DefaultIdType id, VerifyDocumentRequest request, ISender sender) =>
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

    }
}

public record VerifyDocumentRequest(DefaultIdType VerifiedById);
