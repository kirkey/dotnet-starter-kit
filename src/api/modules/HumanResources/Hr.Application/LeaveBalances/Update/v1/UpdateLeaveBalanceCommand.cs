namespace FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Update.v1;

public sealed record UpdateLeaveBalanceCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue(null)] decimal? AccruedDays = null,
    [property: DefaultValue(null)] decimal? TakenDays = null) : IRequest<UpdateLeaveBalanceResponse>;

