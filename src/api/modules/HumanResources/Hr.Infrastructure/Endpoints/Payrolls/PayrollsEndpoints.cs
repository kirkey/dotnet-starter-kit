using FSH.Starter.WebApi.HumanResources.Application.Payrolls.CompleteProcessing.v1;
using FSH.Starter.WebApi.HumanResources.Application.Payrolls.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.Payrolls.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.Payrolls.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Payrolls.MarkAsPaid.v1;
using FSH.Starter.WebApi.HumanResources.Application.Payrolls.Post.v1;
using FSH.Starter.WebApi.HumanResources.Application.Payrolls.Process.v1;
using FSH.Starter.WebApi.HumanResources.Application.Payrolls.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.Payrolls.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Payrolls;

/// <summary>
/// Endpoint configuration for Payrolls module.
/// Supports the complete payroll workflow including creation, processing, GL posting, and payment marking.
/// </summary>
public class PayrollsEndpoints() : CarterModule("humanresources")
{
    /// <summary>
    /// Maps all Payrolls endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/payrolls").WithTags("payrolls");

        group.MapPost("/", async (CreatePayrollCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute("GetPayroll", new { id = response.Id }, response);
            })
            .WithName("CreatePayrollEndpoint")
            .WithSummary("Creates a new payroll period")
            .WithDescription("Creates a new payroll period for processing employee pay. Payroll is created in Draft status and must be processed before GL posting.")
            .Produces<CreatePayrollResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new GetPayrollRequest(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetPayrollEndpoint")
            .WithSummary("Gets a payroll period by ID")
            .WithDescription("Retrieves detailed information about a specific payroll period including totals, status, and GL posting details.")
            .Produces<PayrollResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdatePayrollCommand request, ISender mediator) =>
            {
                var updateRequest = request with { Id = id };
                var response = await mediator.Send(updateRequest).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdatePayrollEndpoint")
            .WithSummary("Updates a payroll period")
            .WithDescription("Updates payroll period details such as notes. Locked payrolls cannot be updated.")
            .Produces<UpdatePayrollResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new DeletePayrollCommand(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeletePayrollEndpoint")
            .WithSummary("Deletes a payroll period")
            .WithDescription("Deletes a payroll period. Only Draft payrolls can be deleted. Processed or posted payrolls cannot be deleted.")
            .Produces<DeletePayrollResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchPayrollsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchPayrollsEndpoint")
            .WithSummary("Searches payroll periods")
            .WithDescription("Searches and filters payroll periods by date range, status, and pay frequency with pagination support.")
            .Produces<PagedList<PayrollResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapPost("/{id}/process", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new ProcessPayrollCommand(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Accepted($"/hr/payrolls/{response.Id}", response);
            })
            .WithName("ProcessPayrollEndpoint")
            .WithSummary("Processes a payroll period")
            .WithDescription("Initiates processing of a Draft payroll period. Calculates pay totals for all lines. Transitions to Processing status.")
            .Produces<ProcessPayrollResponse>(StatusCodes.Status202Accepted)
            .RequirePermission(FshPermission.NameFor(FshActions.Process, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapPost("/{id}/complete-processing", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new CompletePayrollProcessingCommand(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("CompletePayrollProcessingEndpoint")
            .WithSummary("Completes payroll processing")
            .WithDescription("Completes processing of a payroll period and transitions to Processed status. Ready for GL posting.")
            .Produces<CompletePayrollProcessingResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapPost("/{id}/post", async (DefaultIdType id, PostPayrollCommand command, ISender mediator) =>
            {
                var request = command with { Id = id };
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("PostPayrollEndpoint")
            .WithSummary("Posts a payroll to the general ledger")
            .WithDescription("Posts a processed payroll to the GL with the specified journal entry ID. Locks payroll from further editing. Transitions to Posted status.")
            .Produces<PostPayrollResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapPost("/{id}/mark-as-paid", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new MarkPayrollAsPaidCommand(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("MarkPayrollAsPaidEndpoint")
            .WithSummary("Marks a payroll as paid")
            .WithDescription("Marks a posted payroll as paid. Records payment date and transitions to Paid status.")
            .Produces<MarkPayrollAsPaidResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.MarkAsPaid, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}

