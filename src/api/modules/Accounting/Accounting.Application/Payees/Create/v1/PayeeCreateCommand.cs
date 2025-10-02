using System.ComponentModel;

namespace Accounting.Application.Payees.Create.v1;

/// <summary>
/// Command for creating a new payee entity in the accounting system.
/// Follows the CQRS pattern for command operations with comprehensive validation.
/// </summary>
/// <param name="PayeeCode">Unique code identifying the payee (e.g., "VEND001", "UTIL-ELEC").</param>
/// <param name="Name">The name of the payee or vendor company.</param>
/// <param name="Address">Optional mailing or physical address for correspondence and check printing.</param>
/// <param name="ExpenseAccountCode">Optional default expense account code for automated journal entries.</param>
/// <param name="Tin">Optional tax identification number for 1099 reporting and compliance.</param>
/// <param name="Description">Optional detailed description of the payee's business or services.</param>
/// <param name="Notes">Optional additional notes or comments about the payee.</param>
public sealed record PayeeCreateCommand(
    [property: DefaultValue("VEND001")] string PayeeCode,
    [property: DefaultValue("ABC Company")] string Name,
    [property: DefaultValue("123 Business St, City, ST 12345")] string? Address = null,
    [property: DefaultValue("5100")] string? ExpenseAccountCode = null,
    [property: DefaultValue("12-3456789")] string? Tin = null,
    [property: DefaultValue("Primary office supply vendor")] string? Description = null,
    [property: DefaultValue("Preferred vendor for office supplies")] string? Notes = null) : IRequest<PayeeCreateResponse>;
