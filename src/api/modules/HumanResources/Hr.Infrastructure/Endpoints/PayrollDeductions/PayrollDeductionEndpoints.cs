using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollDeductions;

/// <summary>
/// Endpoint configuration for Payroll Deductions module.
/// </summary>
public class PayrollDeductionEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Payroll Deductions endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/payroll-deductions").WithTags("payroll-deductions");

        group.MapPost("/", async (CreatePayrollDeductionCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("CreatePayrollDeduction")
            .WithSummary("Create a new payroll deduction")
            .WithDescription("Creates a new payroll deduction configuration for employees per Philippine Labor Code")
            .Produces<CreatePayrollDeductionResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetPayrollDeductionRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetPayrollDeduction")
            .WithSummary("Get a payroll deduction by ID")
            .WithDescription("Retrieves a specific payroll deduction by its unique identifier")
            .Produces<PayrollDeductionResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdatePayrollDeductionCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdatePayrollDeduction")
            .WithSummary("Update a payroll deduction")
            .WithDescription("Updates an existing payroll deduction configuration")
            .Produces<UpdatePayrollDeductionResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeletePayrollDeductionCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeletePayrollDeduction")
            .WithSummary("Delete a payroll deduction")
            .WithDescription("Deletes a payroll deduction by its unique identifier")
            .Produces<DeletePayrollDeductionResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchPayrollDeductionsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchPayrollDeductions")
            .WithSummary("Searches payroll deductions")
            .WithDescription("Searches and filters payroll deductions by type, employee, department, authorization status with pagination support.")
            .Produces<PagedList<PayrollDeductionResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}
