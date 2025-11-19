namespace FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Create.v1;

/// <summary>
/// Command to create a new payroll line.
/// </summary>
public sealed record CreatePayrollLineCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType PayrollId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType EmployeeId,
    [property: DefaultValue(160)] decimal RegularHours = 160,
    [property: DefaultValue(0)] decimal OvertimeHours = 0) : IRequest<CreatePayrollLineResponse>;

