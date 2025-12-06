using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.CreditBureauInquiries.Complete.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CreditBureauInquiries.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CreditBureauInquiries.Get.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CreditBureauInquiryEndpoints() : CarterModule("microfinance")
{

    private const string CompleteInquiry = "CompleteInquiry";
    private const string CreateCreditBureauInquiry = "CreateCreditBureauInquiry";
    private const string GetCreditBureauInquiry = "GetCreditBureauInquiry";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/credit-bureau-inquiries").WithTags("Credit Bureau Inquiries");

        group.MapPost("/", async (CreateCreditBureauInquiryCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/credit-bureau-inquiries/{result.Id}", result);
        })
        .WithName(CreateCreditBureauInquiry)
        .WithSummary("Create a new credit bureau inquiry")
        .Produces<CreateCreditBureauInquiryResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCreditBureauInquiryRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetCreditBureauInquiry)
        .WithSummary("Get credit bureau inquiry by ID")
        .Produces<CreditBureauInquiryResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/complete", async (Guid id, CompleteInquiryRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CompleteInquiryCommand(
                id,
                request.ReferenceNumber,
                request.CreditScore,
                request.CreditReportId));
            return Results.Ok(result);
        })
        .WithName(CompleteInquiry)
        .WithSummary("Complete a credit bureau inquiry")
        .Produces<CompleteInquiryResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Complete, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public record CompleteInquiryRequest(string ReferenceNumber, int? CreditScore, Guid? CreditReportId);
