using FSH.Starter.WebApi.Store.Application.LotNumbers.Update.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.LotNumbers.v1;

public static class UpdateLotNumberEndpoint
{
    internal static RouteHandlerBuilder MapUpdateLotNumberEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateLotNumberCommand request, ISender sender) =>
            {
                if (id != request.Id)
                {
                    return Results.BadRequest("ID in URL does not match ID in request body");
                }

                var response = await sender.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateLotNumberEndpoint))
            .WithSummary("Update a lot number")
            .WithDescription("Updates lot status and quality notes")
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .Produces<UpdateLotNumberResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
