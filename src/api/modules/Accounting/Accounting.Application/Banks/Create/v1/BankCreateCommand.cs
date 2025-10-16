using System.ComponentModel;
using FSH.Framework.Core.Storage.File.Features;

namespace Accounting.Application.Banks.Create.v1;

/// <summary>
/// Command for creating a new bank entity in the accounting system.
/// Follows the CQRS pattern for command operations with comprehensive validation.
/// </summary>
/// <param name="BankCode">Unique code identifying the bank (e.g., "BNK001", "CHASE-NYC").</param>
/// <param name="Name">The name of the bank or financial institution.</param>
/// <param name="RoutingNumber">Optional ABA routing number for domestic transfers (9 digits for US banks).</param>
/// <param name="SwiftCode">Optional SWIFT/BIC code for international transfers (8 or 11 characters).</param>
/// <param name="Address">Optional physical address of the bank branch.</param>
/// <param name="ContactPerson">Optional name of the account officer or primary contact.</param>
/// <param name="PhoneNumber">Optional bank phone number for inquiries.</param>
/// <param name="Email">Optional bank email address for correspondence.</param>
/// <param name="Website">Optional bank website URL for online banking.</param>
/// <param name="Description">Optional detailed description of the bank or banking relationship.</param>
/// <param name="Notes">Optional additional notes or comments about the bank.</param>
/// <param name="ImageUrl">Optional image URL for the bank logo.</param>
public sealed record BankCreateCommand(
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
    [property: DefaultValue(null)] string? ImageUrl = null) : IRequest<BankCreateResponse>
{
    /// <summary>
    /// Optional image payload uploaded by the client. When provided, the image is uploaded to storage and ImageUrl is set from the saved file name.
    /// </summary>
    public FileUploadCommand? Image { get; init; }
}

