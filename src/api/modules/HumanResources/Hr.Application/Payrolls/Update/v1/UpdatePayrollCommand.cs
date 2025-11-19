namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.Update.v1;

/// <summary>
/// Command to update a payroll record.
/// </summary>
public sealed record UpdatePayrollCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue(null)] string? Status = null,
    [property: DefaultValue(null)] string? JournalEntryId = null,
    [property: DefaultValue(null)] string? Notes = null) : IRequest<UpdatePayrollResponse>;

