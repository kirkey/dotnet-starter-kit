using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Reject.v1;
using FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Verify.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class KycDocumentEndpoints() : CarterModule("microfinance")
{

    private const string CreateKycDocument = "CreateKycDocument";
    private const string GetKycDocument = "GetKycDocument";
    private const string RejectKycDocument = "RejectKycDocument";
    private const string VerifyKycDocument = "VerifyKycDocument";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/kyc-documents").WithTags("KYC Documents");

        group.MapPost("/", async (CreateKycDocumentCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/kyc-documents/{result.Id}", result);
        })
        .WithName(CreateKycDocument)
        .WithSummary("Upload a new KYC document")
        .Produces<CreateKycDocumentResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetKycDocumentRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetKycDocument)
        .WithSummary("Get KYC document by ID")
        .Produces<KycDocumentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/verify", async (Guid id, VerifyKycDocumentRequest request, ISender sender) =>
        {
            var command = new VerifyKycDocumentCommand(id, request.VerifiedById, request.Notes);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName(VerifyKycDocument)
        .WithSummary("Verify KYC document")
        .Produces<VerifyKycDocumentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/reject", async (Guid id, RejectKycDocumentRequest request, ISender sender) =>
        {
            var command = new RejectKycDocumentCommand(id, request.RejectedById, request.Reason);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName(RejectKycDocument)
        .WithSummary("Reject KYC document")
        .Produces<RejectKycDocumentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Reject, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public sealed record VerifyKycDocumentRequest(Guid VerifiedById, string? Notes);
public sealed record RejectKycDocumentRequest(Guid RejectedById, string Reason);
