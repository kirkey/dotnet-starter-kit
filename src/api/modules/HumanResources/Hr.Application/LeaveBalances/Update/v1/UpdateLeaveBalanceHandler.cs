namespace FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Update.v1;

public sealed class UpdateLeaveBalanceHandler(
    ILogger<UpdateLeaveBalanceHandler> logger,
    [FromKeyedServices("hr:leavebalances")] IRepository<LeaveBalance> repository)
    : IRequestHandler<UpdateLeaveBalanceCommand, UpdateLeaveBalanceResponse>
{
    public async Task<UpdateLeaveBalanceResponse> Handle(
        UpdateLeaveBalanceCommand request,
        CancellationToken cancellationToken)
    {
        var balance = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (balance is null)
            throw new NotFoundException($"Leave balance not found: {request.Id}");

        if (request.AccruedDays.HasValue)
            balance.AddAccrual(request.AccruedDays.Value);

        if (request.TakenDays.HasValue)
            balance.RecordLeave(request.TakenDays.Value);

        await repository.UpdateAsync(balance, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Leave balance {LeaveBalanceId} updated successfully", balance.Id);

        return new UpdateLeaveBalanceResponse(balance.Id);
    }
}

