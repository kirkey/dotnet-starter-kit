namespace FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Create.v1;

public sealed record CreateLeaveTypeCommand(
    [property: DefaultValue("Vacation")] string LeaveName,
    [property: DefaultValue(20)] decimal AnnualAllowance,
    [property: DefaultValue("Monthly")] string AccrualFrequency = "Monthly",
    [property: DefaultValue(true)] bool IsPaid = true,
    [property: DefaultValue(true)] bool RequiresApproval = true,
    [property: DefaultValue(0)] decimal MaxCarryoverDays = 0,
    [property: DefaultValue(null)] int? CarryoverExpiryMonths = null,
    [property: DefaultValue(null)] int? MinimumNoticeDay = null,
    [property: DefaultValue(null)] string? Description = null) : IRequest<CreateLeaveTypeResponse>;

