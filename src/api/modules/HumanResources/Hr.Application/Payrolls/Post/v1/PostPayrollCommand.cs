namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.Post.v1;

/// <summary>
/// Command to post a payroll to the general ledger.
/// Transitions payroll from Processed to Posted status and locks for editing.
/// </summary>
public sealed record PostPayrollCommand(
    DefaultIdType Id,
    [property: DefaultValue("")] string JournalEntryId = ""
) : IRequest<PostPayrollResponse>;

/// <summary>
/// Response for posting payroll to GL.
/// </summary>
public sealed record PostPayrollResponse(
    DefaultIdType Id,
    string Status,
    DateTime PostedDate,
    string JournalEntryId);

