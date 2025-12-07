namespace FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Delete.v1;

public sealed class DeleteLeaveBalanceHandler(
    ILogger<DeleteLeaveBalanceHandler> logger,
    [FromKeyedServices("hr:leavebalances")] IRepository<LeaveBalance> repository)
    : IRequestHandler<DeleteLeaveBalanceCommand, DeleteLeaveBalanceResponse>
{
    public async Task<DeleteLeaveBalanceResponse> Handle(
        DeleteLeaveBalanceCommand request,
        CancellationToken cancellationToken)
    {
        var balance = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (balance is null)
            throw new NotFoundException($"Leave balance not found: {request.Id}");

        await repository.DeleteAsync(balance, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Leave balance {LeaveBalanceId} deleted successfully", balance.Id);

        return new DeleteLeaveBalanceResponse(balance.Id);
    }
}

