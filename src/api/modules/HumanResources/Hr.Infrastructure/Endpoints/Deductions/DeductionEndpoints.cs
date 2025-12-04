using Carter;
using FSH.Starter.WebApi.HumanResources.Application.Deductions.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.Deductions.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.Deductions.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Deductions.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.Deductions.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Deductions;

/// <summary>
/// Endpoint routes for managing deduction master data (loans, cash advances, uniform deductions, etc).
/// </summary>
public class DeductionEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Deduction endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/deductions").WithTags("deductions");

        group.MapPost("/", async (CreateDeductionCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute("GetDeduction", new { id = response.Id }, response);
            })
            .WithName("CreateDeduction")
            .WithSummary("Create Deduction Type")
            .WithDescription("Creates a new deduction type (loan, cash advance, uniform, etc) with recovery rules per Philippines Labor Code Art 113.")
            .Produces<CreateDeductionResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new GetDeductionRequest(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetDeduction")
            .WithSummary("Get Deduction Details")
            .WithDescription("Retrieves detailed information for the specified deduction type including recovery rules and compliance settings.")
            .Produces<DeductionResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchDeductionsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchDeductions")
            .WithSummary("Search Deductions")
            .WithDescription("Search deduction types by type, recovery method, and active status with pagination.")
            .Produces<PagedList<DeductionDto>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateDeductionCommand body, ISender mediator) =>
            {
                if (body.Id != id)
                    return Results.BadRequest(new { title = "ID mismatch." });

                var response = await mediator.Send(body).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateDeduction")
            .WithSummary("Update Deduction Type")
            .WithDescription("Updates deduction type details, recovery rules, and compliance settings.")
            .Produces<UpdateDeductionResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var command = new DeleteDeductionCommand(id);
                var response = await mediator.Send(command).ConfigureAwait(false);
                return response.IsDeleted ? Results.Ok(response) : Results.NotFound();
            })
            .WithName("DeleteDeduction")
            .WithSummary("Delete Deduction Type")
            .WithDescription("Deletes a deduction type from the master data.")
            .Produces<DeleteDeductionResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}

