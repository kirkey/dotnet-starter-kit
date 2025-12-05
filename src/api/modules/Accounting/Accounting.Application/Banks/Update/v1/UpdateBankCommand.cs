using System.ComponentModel;
using FSH.Framework.Core.Storage.File.Features;

namespace Accounting.Application.Banks.Update.v1;

/// <summary>
/// Command for updating an existing bank entity in the accounting system.
/// Follows the CQRS pattern for command operations with comprehensive validation.
/// </summary>
public sealed record UpdateBankCommand : IRequest<BankUpdateResponse>
{
    /// <summary>
    /// The unique identifier of the bank to update.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Updated unique code identifying the bank.
    /// </summary>
    [DefaultValue("BNK001")]
    public string BankCode { get; init; } = null!;

    /// <summary>
    /// Updated name of the bank or financial institution.
    /// </summary>
    [DefaultValue("Chase Bank")]
    public string Name { get; init; } = null!;

    /// <summary>
    /// Updated ABA routing number for domestic transfers.
    /// </summary>
    [DefaultValue("021000021")]
    public string? RoutingNumber { get; init; }

    /// <summary>
    /// Updated SWIFT/BIC code for international transfers.
    /// </summary>
    [DefaultValue("CHASUS33")]
    public string? SwiftCode { get; init; }

    /// <summary>
    /// Updated physical address of the bank branch.
    /// </summary>
    [DefaultValue("123 Bank St, New York, NY 10001")]
    public string? Address { get; init; }

    /// <summary>
    /// Updated name of the account officer or primary contact.
    /// </summary>
    [DefaultValue("John Smith")]
    public string? ContactPerson { get; init; }

    /// <summary>
    /// Updated bank phone number for inquiries.
    /// </summary>
    [DefaultValue("+1-555-0100")]
    public string? PhoneNumber { get; init; }

    /// <summary>
    /// Updated bank email address for correspondence.
    /// </summary>
    [DefaultValue("accounts@bank.com")]
    public string? Email { get; init; }

    /// <summary>
    /// Updated bank website URL for online banking.
    /// </summary>
    [DefaultValue("https://www.bank.com")]
    public string? Website { get; init; }

    /// <summary>
    /// Updated detailed description of the bank or banking relationship.
    /// </summary>
    [DefaultValue("Primary banking institution")]
    public string? Description { get; init; }

    /// <summary>
    /// Updated additional notes or comments about the bank.
    /// </summary>
    [DefaultValue("Monthly service fee: $25")]
    public string? Notes { get; init; }

    /// <summary>
    /// Updated image URL for the bank logo.
    /// </summary>
    [DefaultValue(null)]
    public string? ImageUrl { get; init; }

    /// <summary>
    /// Optional image payload uploaded by the client. When provided, the image will be saved to storage and ImageUrl will be set to the saved file URL.
    /// </summary>
    public FileUploadCommand? Image { get; init; }
}
