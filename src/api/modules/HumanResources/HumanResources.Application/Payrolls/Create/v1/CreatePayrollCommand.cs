namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.Create.v1;

/// <summary>
/// Command to create a new payroll period.
/// </summary>
public sealed record CreatePayrollCommand(
    [property: DefaultValue("2025-11-01")] DateTime StartDate,
    [property: DefaultValue("2025-11-30")] DateTime EndDate,
    [property: DefaultValue("Monthly")] string PayFrequency = "Monthly",
    [property: DefaultValue(null)] string? Notes = null) : IRequest<CreatePayrollResponse>;

