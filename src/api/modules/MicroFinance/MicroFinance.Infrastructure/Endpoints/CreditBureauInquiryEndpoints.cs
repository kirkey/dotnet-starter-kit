using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.CreditBureauInquiries.Complete.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CreditBureauInquiries.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CreditBureauInquiries.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CreditBureauInquiryEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/credit-bureau-inquiries").WithTags("Credit Bureau Inquiries");

        group.MapPost("/", async (CreateCreditBureauInquiryCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/credit-bureau-inquiries/{result.Id}", result);
        })
        .WithName("CreateCreditBureauInquiry")
        .WithSummary("Create a new credit bureau inquiry")
        .Produces<CreateCreditBureauInquiryResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCreditBureauInquiryRequest(id));
            return Results.Ok(result);
        })
        .WithName("GetCreditBureauInquiry")
        .WithSummary("Get credit bureau inquiry by ID")
        .Produces<CreditBureauInquiryResponse>();

        group.MapPost("/{id:guid}/complete", async (Guid id, CompleteInquiryRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CompleteInquiryCommand(
                id,
                request.ReferenceNumber,
                request.CreditScore,
                request.CreditReportId));
            return Results.Ok(result);
        })
        .WithName("CompleteInquiry")
        .WithSummary("Complete a credit bureau inquiry")
        .Produces<CompleteInquiryResponse>();

    }
}

public record CompleteInquiryRequest(string ReferenceNumber, int? CreditScore, Guid? CreditReportId);
