using FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollLines;

/// <summary>
/// Endpoint configuration for Payroll Lines module.
/// Each payroll line represents one employee's pay calculation for a payroll period.
/// </summary>
public class PayrollLinesEndpoints() : CarterModule("humanresources")
{
    /// <summary>
    /// Maps all Payroll Lines endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/payroll-lines").WithTags("payroll-lines");

        group.MapPost("/", async (CreatePayrollLineCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute("GetPayrollLine", new { id = response.Id }, response);
            })
            .WithName("CreatePayrollLine")
            .WithSummary("Creates a new payroll line")
            .WithDescription("Creates a new payroll line for an employee within a payroll period. Contains hours worked and pay calculations.")
            .Produces<CreatePayrollLineResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new GetPayrollLineRequest(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetPayrollLine")
            .WithSummary("Gets a payroll line by ID")
            .WithDescription("Retrieves detailed information about a specific payroll line including hours, earnings, taxes, and deductions.")
            .Produces<PayrollLineResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdatePayrollLineCommand request, ISender mediator) =>
            {
                var updateRequest = request with { Id = id };
                var response = await mediator.Send(updateRequest).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdatePayrollLine")
            .WithSummary("Updates a payroll line")
            .WithDescription("Updates a payroll line's hours and pay information. Only draft payroll lines can be updated.")
            .Produces<UpdatePayrollLineResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new DeletePayrollLineCommand(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeletePayrollLine")
            .WithSummary("Deletes a payroll line")
            .WithDescription("Deletes a payroll line. Only lines from draft payrolls can be deleted. Processed payroll lines cannot be deleted.")
            .Produces<DeletePayrollLineResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchPayrollLinesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchPayrollLines")
            .WithSummary("Searches payroll lines")
            .WithDescription("Searches and filters payroll lines by payroll period, employee, and other criteria with pagination support.")
            .Produces<PagedList<PayrollLineResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}

