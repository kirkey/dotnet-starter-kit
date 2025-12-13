using Accounting.Application.AccountsReceivableAccounts.RecordWriteOff.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountsReceivableAccounts.v1;

/// <summary>
/// Endpoint for recording AR write-offs.
/// </summary>
public static class ArAccountRecordWriteOffEndpoint
{
    internal static RouteHandlerBuilder MapArAccountRecordWriteOffEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/write-offs", async (DefaultIdType id, RecordARWriteOffCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var accountId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = accountId, Message = "Write-off recorded successfully" });
            })
            .WithName(nameof(ArAccountRecordWriteOffEndpoint))
            .WithSummary("Record AR bad debt write-off")
            .WithDescription("Records a bad debt write-off for an AR account and updates bad debt statistics")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

