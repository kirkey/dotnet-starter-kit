namespace FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Delete.v1;

public sealed record DeleteLeaveBalanceCommand(DefaultIdType Id) : IRequest<DeleteLeaveBalanceResponse>;

