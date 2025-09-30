using System.ComponentModel;

namespace Accounting.Application.Payees.Update.v1;

/// <summary>
/// Command for updating an existing payee entity in the accounting system.
/// Follows the CQRS pattern for command operations with comprehensive validation.
/// </summary>
/// <param name="Id">The unique identifier of the payee to update.</param>
/// <param name="PayeeCode">Updated unique code identifying the payee.</param>
/// <param name="Name">Updated name of the payee or vendor company.</param>
/// <param name="Address">Updated mailing or physical address.</param>
/// <param name="ExpenseAccountCode">Updated default expense account code.</param>
/// <param name="ExpenseAccountName">Updated expense account name.</param>
/// <param name="Tin">Updated tax identification number.</param>
/// <param name="Description">Updated detailed description of the payee.</param>
/// <param name="Notes">Updated additional notes or comments.</param>
public sealed record PayeeUpdateCommand(
    DefaultIdType Id,
    [property: DefaultValue("VEND001")] string PayeeCode,
    [property: DefaultValue("ABC Company")] string Name,
    [property: DefaultValue("123 Business St, City, ST 12345")] string? Address = null,
    [property: DefaultValue("5100")] string? ExpenseAccountCode = null,
    [property: DefaultValue("Office Supplies Expense")] string? ExpenseAccountName = null,
    [property: DefaultValue("12-3456789")] string? Tin = null,
    [property: DefaultValue("Primary office supply vendor")] string? Description = null,
    [property: DefaultValue("Preferred vendor for office supplies")] string? Notes = null) : IRequest<PayeeUpdateResponse>;
