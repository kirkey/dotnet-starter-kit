using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Complete.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Fail.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Get.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class MobileTransactionEndpoints() : CarterModule("microfinance")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/mobile-transactions").WithTags("Mobile Transactions");

        group.MapPost("/", async (CreateMobileTransactionCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/mobile-transactions/{result.Id}", result);
        })
        .WithName("CreateMobileTransaction")
        .WithSummary("Create a new mobile transaction")
        .Produces<CreateMobileTransactionResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetMobileTransactionRequest(id));
            return Results.Ok(result);
        })
        .WithName("GetMobileTransaction")
        .WithSummary("Get mobile transaction by ID")
        .Produces<MobileTransactionResponse>();

        group.MapPost("/{id:guid}/complete", async (Guid id, CompleteMobileTransactionRequest request, ISender sender) =>
        {
            var command = new CompleteMobileTransactionCommand(id, request.ProviderResponse);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("CompleteMobileTransaction")
        .WithSummary("Complete mobile transaction")
        .Produces<CompleteMobileTransactionResponse>();

        group.MapPost("/{id:guid}/fail", async (Guid id, FailMobileTransactionRequest request, ISender sender) =>
        {
            var command = new FailMobileTransactionCommand(id, request.FailureReason);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("FailMobileTransaction")
        .WithSummary("Mark mobile transaction as failed")
        .Produces<FailMobileTransactionResponse>();

    }
}

public sealed record CompleteMobileTransactionRequest(string ProviderResponse);
public sealed record FailMobileTransactionRequest(string FailureReason);
