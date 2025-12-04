using Carter;
using FSH.Starter.WebApi.Store.Application.LotNumbers.Create.v1;
using FSH.Starter.WebApi.Store.Application.LotNumbers.Delete.v1;
using FSH.Starter.WebApi.Store.Application.LotNumbers.Get.v1;
using FSH.Starter.WebApi.Store.Application.LotNumbers.Search.v1;
using FSH.Starter.WebApi.Store.Application.LotNumbers.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;
using GetLotNumberResponse = FSH.Starter.WebApi.Store.Application.LotNumbers.Get.v1.LotNumberResponse;
using SearchLotNumberResponse = FSH.Starter.WebApi.Store.Application.LotNumbers.Search.v1.LotNumberResponse;

namespace Store.Infrastructure.Endpoints.LotNumbers;

public class LotNumbersEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/lot-numbers").WithTags("lot-numbers");

        group.MapPost("/", async (CreateLotNumberCommand request, ISender sender) =>
        {
            var response = await sender.Send(request);
            return Results.Created($"/api/v1/store/lot-numbers/{response.Id}", response);
        })
        .WithName("CreateLotNumber")
        .WithSummary("Create a new lot number")
        .WithDescription("Creates a new lot/batch number for inventory traceability and expiration management")
        .Produces<CreateLotNumberResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Store))
        .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateLotNumberCommand request, ISender sender) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest("ID in URL does not match ID in request body");
            }

            var response = await sender.Send(request);
            return Results.Ok(response);
        })
        .WithName("UpdateLotNumber")
        .WithSummary("Update a lot number")
        .WithDescription("Updates lot status and quality notes")
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .Produces<UpdateLotNumberResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            await sender.Send(new DeleteLotNumberCommand(id));
            return Results.NoContent();
        })
        .WithName("DeleteLotNumber")
        .WithSummary("Delete a lot number")
        .WithDescription("Removes a lot number from the system")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Store))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var response = await sender.Send(new GetLotNumberCommand(id));
            return Results.Ok(response);
        })
        .WithName("GetLotNumber")
        .WithSummary("Get a lot number by ID")
        .WithDescription("Retrieves detailed information about a specific lot/batch number")
        .Produces<GetLotNumberResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchLotNumbersCommand command, ISender sender) =>
        {
            var response = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("SearchLotNumbers")
        .WithSummary("Search lot numbers")
        .WithDescription("Search and filter lot numbers with expiration tracking and pagination support")
        .Produces<PagedList<SearchLotNumberResponse>>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);
    }
}
