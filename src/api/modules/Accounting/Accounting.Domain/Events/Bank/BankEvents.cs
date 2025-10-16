namespace Accounting.Domain.Events.Bank;

/// <summary>
/// Domain event raised when a new bank is created in the system.
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
public record BankCreated(
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
    string? Notes) : DomainEvent;

/// <summary>
/// Domain event raised when a bank is updated.
/// </summary>
/// <param name="Id">The unique identifier of the bank.</param>
/// <param name="Bank">The updated bank entity.</param>
public record BankUpdated(DefaultIdType Id, Entities.Bank Bank) : DomainEvent;

/// <summary>
/// Domain event raised when a bank is activated.
/// </summary>
/// <param name="Id">The unique identifier of the bank.</param>
/// <param name="Bank">The activated bank entity.</param>
public record BankActivated(DefaultIdType Id, Entities.Bank Bank) : DomainEvent;

/// <summary>
/// Domain event raised when a bank is deactivated.
/// </summary>
/// <param name="Id">The unique identifier of the bank.</param>
/// <param name="Bank">The deactivated bank entity.</param>
public record BankDeactivated(DefaultIdType Id, Entities.Bank Bank) : DomainEvent;

/// <summary>
/// Domain event raised when a bank is deleted from the system.
/// </summary>
/// <param name="Id">The unique identifier of the deleted bank.</param>
public record BankDeleted(DefaultIdType Id) : DomainEvent;

