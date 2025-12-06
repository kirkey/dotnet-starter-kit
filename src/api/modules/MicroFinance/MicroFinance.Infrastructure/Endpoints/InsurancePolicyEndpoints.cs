using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Cancel.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.RecordPremium.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class InsurancePolicyEndpoints() : CarterModule("microfinance")
{

    private const string ActivateInsurancePolicy = "ActivateInsurancePolicy";
    private const string CancelInsurancePolicy = "CancelInsurancePolicy";
    private const string CreateInsurancePolicy = "CreateInsurancePolicy";
    private const string GetInsurancePolicy = "GetInsurancePolicy";
    private const string RecordPremiumPayment = "RecordPremiumPayment";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/insurance-policies").WithTags("insurance-policies");

        // CRUD Operations
        group.MapPost("/", async (CreateInsurancePolicyCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/insurance-policies/{response.Id}", response);
            })
            .WithName(CreateInsurancePolicy)
            .WithSummary("Creates a new insurance policy for a member")
            .Produces<CreateInsurancePolicyResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetInsurancePolicyRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetInsurancePolicy)
            .WithSummary("Gets an insurance policy by ID")
            .Produces<InsurancePolicyResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        // Lifecycle Operations
        group.MapPost("/{id:guid}/activate", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new ActivateInsurancePolicyCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(ActivateInsurancePolicy)
            .WithSummary("Activates an insurance policy after first premium")
            .Produces<ActivateInsurancePolicyResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/cancel", async (Guid id, CancelInsurancePolicyCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(CancelInsurancePolicy)
            .WithSummary("Cancels an insurance policy")
            .Produces<CancelInsurancePolicyResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Cancel, FshResources.MicroFinance))
            .MapToApiVersion(1);

        // Premium Payments
        group.MapPost("/{id:guid}/record-premium", async (Guid id, RecordPremiumPaymentCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(RecordPremiumPayment)
            .WithSummary("Records a premium payment on the policy")
            .Produces<RecordPremiumPaymentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

    }
}
