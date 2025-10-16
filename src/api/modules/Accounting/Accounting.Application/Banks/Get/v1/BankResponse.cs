namespace Accounting.Application.Banks.Get.v1;

/// <summary>
/// Response containing bank details.
/// </summary>
/// <param name="Id">The unique identifier of the bank.</param>
/// <param name="BankCode">The unique bank code.</param>
/// <param name="Name">The bank name.</param>
/// <param name="RoutingNumber">The ABA routing number.</param>
/// <param name="SwiftCode">The SWIFT/BIC code.</param>
/// <param name="Address">The bank address.</param>
/// <param name="ContactPerson">The contact person name.</param>
/// <param name="PhoneNumber">The phone number.</param>
/// <param name="Email">The email address.</param>
/// <param name="Website">The website URL.</param>
/// <param name="Description">The description.</param>
/// <param name="Notes">The notes.</param>
/// <param name="IsActive">Whether the bank is active.</param>
/// <param name="ImageUrl">The image URL.</param>
public sealed record BankResponse(
    DefaultIdType Id,
    string BankCode,
    string Name,
    string? RoutingNumber,
    string? SwiftCode,
    string? Address,
    string? ContactPerson,
    string? PhoneNumber,
    string? Email,
    string? Website,
    string? Description,
    string? Notes,
    bool IsActive,
    string? ImageUrl);

