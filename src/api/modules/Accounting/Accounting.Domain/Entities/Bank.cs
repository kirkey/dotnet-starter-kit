using Accounting.Domain.Events.Bank;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a bank or financial institution used for managing bank accounts in the accounting system.
/// </summary>
/// <remarks>
/// Use cases:
/// - Manage bank master data for bank account management and check processing.
/// - Store bank contact information, routing numbers, and SWIFT codes for wire transfers.
/// - Support bank reconciliation by linking bank accounts to financial institutions.
/// - Enable check printing with proper bank details on check layouts.
/// - Track banking relationships and account officer information.
/// - Facilitate electronic fund transfers with proper routing information.
/// - Support multi-bank operations for organizations with accounts at multiple institutions.
/// - Enable bank address management for correspondence and audit documentation.
/// 
/// Default values:
/// - BankCode: required unique identifier (example: "BNK001", "CHASE-NYC")
/// - RoutingNumber: optional ABA routing number (example: "021000021" for Chase)
/// - SwiftCode: optional SWIFT/BIC code for international transfers (example: "CHASUS33")
/// - Address: optional bank branch address (example: "123 Bank St, New York, NY 10001")
/// - ContactPerson: optional account officer name (example: "John Smith")
/// - PhoneNumber: optional bank phone number (example: "+1-555-0100")
/// - Email: optional bank email address (example: "accounts@bankname.com")
/// - Website: optional bank website URL (example: "https://www.bankname.com")
/// - IsActive: true (new banks are active by default)
/// - Name: required bank name (example: "Chase Bank", "Bank of America")
/// - Description: optional detailed description (example: "Primary banking institution for operations")
/// 
/// Business rules:
/// - BankCode must be unique within the system
/// - RoutingNumber format should be validated (9 digits for US banks)
/// - SwiftCode format should be validated (8 or 11 characters)
/// - Cannot deactivate banks with active bank accounts or pending transactions
/// - PhoneNumber format should be validated for consistency
/// - Email format must be valid when provided
/// - Name is required and must be unique to prevent duplicate bank entries
/// - RoutingNumber must be unique if provided (one bank per routing number)
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.Bank.BankCreated"/>
/// <seealso cref="Accounting.Domain.Events.Bank.BankUpdated"/>
/// <seealso cref="Accounting.Domain.Events.Bank.BankActivated"/>
/// <seealso cref="Accounting.Domain.Events.Bank.BankDeactivated"/>
public class Bank : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique code identifying the bank.
    /// Example: "BNK001", "CHASE-NYC", "BOA-LA".
    /// </summary>
    public string BankCode { get; private set; } = string.Empty;

    /// <summary>
    /// Bank or financial institution name.
    /// Example: "Chase Bank", "Bank of America", "Wells Fargo".
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// ABA routing number for domestic wire transfers and ACH transactions.
    /// Example: "021000021" (Chase), "026009593" (Bank of America).
    /// Format: 9 digits for US banks.
    /// </summary>
    public string? RoutingNumber { get; private set; }

    /// <summary>
    /// SWIFT/BIC code for international wire transfers.
    /// Example: "CHASUS33" (Chase), "BOFAUS3N" (Bank of America).
    /// Format: 8 or 11 characters.
    /// </summary>
    public string? SwiftCode { get; private set; }

    /// <summary>
    /// Physical address of the bank branch.
    /// Example: "123 Bank St, New York, NY 10001".
    /// </summary>
    public string? Address { get; private set; }

    /// <summary>
    /// Name of the account officer or primary contact at the bank.
    /// Example: "John Smith", "Jane Doe - Relationship Manager".
    /// </summary>
    public string? ContactPerson { get; private set; }

    /// <summary>
    /// Bank phone number for inquiries and support.
    /// Example: "+1-555-0100", "1-800-BANK-123".
    /// </summary>
    public string? PhoneNumber { get; private set; }

    /// <summary>
    /// Bank email address for electronic correspondence.
    /// Example: "accounts@bankname.com", "support@bank.com".
    /// </summary>
    public string? Email { get; private set; }

    /// <summary>
    /// Bank website URL for online banking and information.
    /// Example: "https://www.chase.com", "https://www.bankofamerica.com".
    /// </summary>
    public string? Website { get; private set; }

    /// <summary>
    /// Optional detailed description of the bank or banking relationship.
    /// Example: "Primary banking institution for operations", "Payroll bank account".
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Optional additional notes or comments about the bank.
    /// Example: "Monthly service fee: $25", "Minimum balance requirement: $10,000".
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// Whether the bank is active and can be used for bank account operations. Default: true.
    /// Used to retire banks without deleting historical data.
    /// </summary>
    public bool IsActive { get; private set; } = true;

    private Bank()
    {
        // EF Core requires a parameterless constructor for entity instantiation
    }

    private Bank(
        string bankCode,
        string name,
        string? routingNumber,
        string? swiftCode,
        string? address,
        string? contactPerson,
        string? phoneNumber,
        string? email,
        string? website,
        string? description,
        string? notes,
        string? imageUrl)
    {
        BankCode = bankCode.Trim();
        Name = name.Trim();
        RoutingNumber = routingNumber?.Trim();
        SwiftCode = swiftCode?.Trim();
        Address = address?.Trim();
        ContactPerson = contactPerson?.Trim();
        PhoneNumber = phoneNumber?.Trim();
        Email = email?.Trim();
        Website = website?.Trim();
        Description = description?.Trim();
        Notes = notes?.Trim();
        ImageUrl = imageUrl;
    }

    /// <summary>
    /// Create a new bank entity with comprehensive banking information.
    /// </summary>
    /// <param name="bankCode">Unique bank code.</param>
    /// <param name="name">Bank name.</param>
    /// <param name="routingNumber">Optional ABA routing number.</param>
    /// <param name="swiftCode">Optional SWIFT/BIC code.</param>
    /// <param name="address">Optional bank address.</param>
    /// <param name="contactPerson">Optional contact person name.</param>
    /// <param name="phoneNumber">Optional phone number.</param>
    /// <param name="email">Optional email address.</param>
    /// <param name="website">Optional website URL.</param>
    /// <param name="description">Optional description.</param>
    /// <param name="notes">Optional notes.</param>
    /// <param name="imageUrl">Optional image URL.</param>
    /// <returns>New bank entity.</returns>
    public static Bank Create(
        string bankCode,
        string name,
        string? routingNumber,
        string? swiftCode,
        string? address,
        string? contactPerson,
        string? phoneNumber,
        string? email,
        string? website,
        string? description,
        string? notes,
        string? imageUrl = null)
    {
        var bank = new Bank(
            bankCode,
            name,
            routingNumber,
            swiftCode,
            address,
            contactPerson,
            phoneNumber,
            email,
            website,
            description,
            notes,
            imageUrl);

        bank.QueueDomainEvent(new BankCreated(
            bank.Id,
            bank.BankCode,
            bank.Name,
            bank.RoutingNumber,
            bank.SwiftCode,
            bank.Address,
            bank.ContactPerson,
            bank.PhoneNumber,
            bank.Email,
            bank.Website,
            bank.Description,
            bank.Notes));

        return bank;
    }

    /// <summary>
    /// Update the bank information; trims inputs and raises domain event if changes are made.
    /// </summary>
    /// <param name="bankCode">Updated bank code.</param>
    /// <param name="name">Updated bank name.</param>
    /// <param name="routingNumber">Updated routing number.</param>
    /// <param name="swiftCode">Updated SWIFT code.</param>
    /// <param name="address">Updated address.</param>
    /// <param name="contactPerson">Updated contact person.</param>
    /// <param name="phoneNumber">Updated phone number.</param>
    /// <param name="email">Updated email.</param>
    /// <param name="website">Updated website.</param>
    /// <param name="description">Updated description.</param>
    /// <param name="notes">Updated notes.</param>
    /// <param name="imageUrl">Updated image URL.</param>
    /// <returns>Updated bank entity.</returns>
    public Bank Update(
        string? bankCode,
        string? name,
        string? routingNumber,
        string? swiftCode,
        string? address,
        string? contactPerson,
        string? phoneNumber,
        string? email,
        string? website,
        string? description,
        string? notes,
        string? imageUrl)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(bankCode) && !string.Equals(BankCode, bankCode, StringComparison.OrdinalIgnoreCase))
        {
            BankCode = bankCode.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name.Trim();
            isUpdated = true;
        }

        if (!string.Equals(RoutingNumber, routingNumber, StringComparison.OrdinalIgnoreCase))
        {
            RoutingNumber = routingNumber?.Trim();
            isUpdated = true;
        }

        if (!string.Equals(SwiftCode, swiftCode, StringComparison.OrdinalIgnoreCase))
        {
            SwiftCode = swiftCode?.Trim();
            isUpdated = true;
        }

        if (!string.Equals(Address, address, StringComparison.OrdinalIgnoreCase))
        {
            Address = address?.Trim();
            isUpdated = true;
        }

        if (!string.Equals(ContactPerson, contactPerson, StringComparison.OrdinalIgnoreCase))
        {
            ContactPerson = contactPerson?.Trim();
            isUpdated = true;
        }

        if (!string.Equals(PhoneNumber, phoneNumber, StringComparison.OrdinalIgnoreCase))
        {
            PhoneNumber = phoneNumber?.Trim();
            isUpdated = true;
        }

        if (!string.Equals(Email, email, StringComparison.OrdinalIgnoreCase))
        {
            Email = email?.Trim();
            isUpdated = true;
        }

        if (!string.Equals(Website, website, StringComparison.OrdinalIgnoreCase))
        {
            Website = website?.Trim();
            isUpdated = true;
        }

        if (!string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description?.Trim();
            isUpdated = true;
        }

        if (!string.Equals(Notes, notes, StringComparison.OrdinalIgnoreCase))
        {
            Notes = notes?.Trim();
            isUpdated = true;
        }

        if (!string.Equals(ImageUrl, imageUrl, StringComparison.OrdinalIgnoreCase))
        {
            ImageUrl = imageUrl;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new BankUpdated(Id, this));
        }

        return this;
    }

    /// <summary>
    /// Activate the bank to allow it to be used for bank account operations.
    /// </summary>
    /// <returns>Updated bank entity.</returns>
    public Bank Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            QueueDomainEvent(new BankActivated(Id, this));
        }

        return this;
    }

    /// <summary>
    /// Deactivate the bank to prevent it from being used for new bank account operations.
    /// Historical data and existing accounts remain accessible.
    /// </summary>
    /// <returns>Updated bank entity.</returns>
    public Bank Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
            QueueDomainEvent(new BankDeactivated(Id, this));
        }

        return this;
    }
}

