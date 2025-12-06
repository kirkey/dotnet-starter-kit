using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Credit.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Debit.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Get.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class MobileWalletEndpoints() : CarterModule("microfinance")
{

    private const string CreateMobileWallet = "CreateMobileWallet";
    private const string CreditMobileWallet = "CreditMobileWallet";
    private const string DebitMobileWallet = "DebitMobileWallet";
    private const string GetMobileWallet = "GetMobileWallet";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/mobile-wallets").WithTags("Mobile Wallets");

        group.MapPost("/", async (CreateMobileWalletCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/mobile-wallets/{result.Id}", result);
        })
        .WithName(CreateMobileWallet)
        .WithSummary("Create a new mobile wallet")
        .Produces<CreateMobileWalletResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetMobileWalletRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetMobileWallet)
        .WithSummary("Get mobile wallet by ID")
        .Produces<MobileWalletResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/credit", async (Guid id, CreditRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreditMobileWalletCommand(id, request.Amount, request.TransactionReference));
            return Results.Ok(result);
        })
        .WithName(CreditMobileWallet)
        .WithSummary("Credit mobile wallet")
        .Produces<CreditMobileWalletResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/debit", async (Guid id, DebitRequest request, ISender sender) =>
        {
            var result = await sender.Send(new DebitMobileWalletCommand(id, request.Amount, request.TransactionReference));
            return Results.Ok(result);
        })
        .WithName(DebitMobileWallet)
        .WithSummary("Debit mobile wallet")
        .Produces<DebitMobileWalletResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public record CreditRequest(decimal Amount, string TransactionReference);
public record DebitRequest(decimal Amount, string TransactionReference);
