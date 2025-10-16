using System.ComponentModel;

namespace Accounting.Application.Banks.Update.v1;

/// <summary>
/// Command for updating an existing bank entity in the accounting system.
/// Follows the CQRS pattern for command operations with comprehensive validation.
/// </summary>
/// <param name="Id">The unique identifier of the bank to update.</param>
/// <param name="BankCode">Updated unique code identifying the bank.</param>
/// <param name="Name">Updated name of the bank or financial institution.</param>
/// <param name="RoutingNumber">Updated ABA routing number for domestic transfers.</param>
/// <param name="SwiftCode">Updated SWIFT/BIC code for international transfers.</param>
/// <param name="Address">Updated physical address of the bank branch.</param>
/// <param name="ContactPerson">Updated name of the account officer or primary contact.</param>
/// <param name="PhoneNumber">Updated bank phone number for inquiries.</param>
/// <param name="Email">Updated bank email address for correspondence.</param>
/// <param name="Website">Updated bank website URL for online banking.</param>
/// <param name="Description">Updated detailed description of the bank or banking relationship.</param>
/// <param name="Notes">Updated additional notes or comments about the bank.</param>
/// <param name="ImageUrl">Updated image URL for the bank logo.</param>
public sealed record BankUpdateCommand(
    DefaultIdType Id,
    [property: DefaultValue("BNK001")] string BankCode,
    [property: DefaultValue("Chase Bank")] string Name,
    [property: DefaultValue("021000021")] string? RoutingNumber = null,
    [property: DefaultValue("CHASUS33")] string? SwiftCode = null,
    [property: DefaultValue("123 Bank St, New York, NY 10001")] string? Address = null,
    [property: DefaultValue("John Smith")] string? ContactPerson = null,
    [property: DefaultValue("+1-555-0100")] string? PhoneNumber = null,
    [property: DefaultValue("accounts@bank.com")] string? Email = null,
    [property: DefaultValue("https://www.bank.com")] string? Website = null,
    [property: DefaultValue("Primary banking institution")] string? Description = null,
    [property: DefaultValue("Monthly service fee: $25")] string? Notes = null,
    [property: DefaultValue(null)] string? ImageUrl = null) : IRequest<BankUpdateResponse>;
