using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a branch or office location of the MFI (Microfinance Institution).
/// Branches serve as operational units for member services, loan disbursement, and collections.
/// </summary>
/// <remarks>
/// Use cases:
/// - Define the MFI's physical presence and service coverage areas.
/// - Organize members, loans, and accounts by geographic location.
/// - Track branch-level performance metrics and targets.
/// - Manage cash vault limits and daily operations.
/// - Establish hierarchical reporting structure (Head Office → Regional → Branch → Sub-Branch).
/// - Enable geographic-based access controls and reporting.
/// 
/// Default values and constraints:
/// - Code: required unique identifier, max 32 characters (example: "BR-KGL-001", "HQ-001")
/// - Name: required branch name, max 128 characters (example: "Kigali Main Branch")
/// - BranchType: required, one of HeadOffice, Regional, Branch, SubBranch, ServiceCenter, MobileUnit, Agency, Kiosk
/// - Status: "Active" by default (UnderConstruction, Active, TemporarilyClosed, PermanentlyClosed)
/// - ParentBranchId: optional, for hierarchical structure
/// - Address, Phone, Email: optional contact information
/// - CashVaultLimit: maximum cash holding limit for security
/// - OpeningDate: date branch started operations
/// 
/// Business rules:
/// - Code must be unique within the system.
/// - HeadOffice branch cannot have a parent branch.
/// - Cannot delete branches with active members, loans, or staff.
/// - Cannot close branches with pending transactions.
/// - Branch hierarchy must not create circular references.
/// - Cash vault limits determine maximum cash holding for insurance compliance.
/// - Status transitions: UnderConstruction → Active → TemporarilyClosed/PermanentlyClosed.
/// </remarks>
/// <seealso cref="Staff"/>
/// <seealso cref="Member"/>
/// <seealso cref="CashVault"/>
/// <seealso cref="BranchTarget"/>
/// <seealso cref="TellerSession"/>
public sealed class Branch : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Maximum length constants for string properties.
    /// </summary>
    public static class MaxLengths
    {
        public const int Code = 32;
        public const int Name = 128;
        public const int Address = 512;
        public const int City = 64;
        public const int State = 64;
        public const int Country = 64;
        public const int PostalCode = 16;
        public const int Phone = 32;
        public const int Email = 128;
        public const int ManagerName = 128;
        public const int ManagerPhone = 32;
        public const int ManagerEmail = 128;
        public const int BranchType = 32;
        public const int Timezone = 64;
    }

    /// <summary>
    /// Branch type indicating the operational scope.
    /// </summary>
    public const string TypeHeadOffice = "HeadOffice";
    public const string TypeRegional = "Regional";
    public const string TypeBranch = "Branch";
    public const string TypeSubBranch = "SubBranch";
    public const string TypeServiceCenter = "ServiceCenter";
    public const string TypeMobileUnit = "MobileUnit";
    public const string TypeAgency = "Agency";
    public const string TypeKiosk = "Kiosk";

    /// <summary>
    /// Status indicating the operational state.
    /// </summary>
    public const string StatusActive = "Active";
    public const string StatusInactive = "Inactive";
    public const string StatusUnderConstruction = "UnderConstruction";
    public const string StatusTemporarilyClosed = "TemporarilyClosed";
    public const string StatusPermanentlyClosed = "PermanentlyClosed";

    /// <summary>
    /// Unique branch code for identification.
    /// </summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>
    /// Physical address of the branch.
    /// </summary>
    public string? Address { get; private set; }

    /// <summary>
    /// City where the branch is located.
    /// </summary>
    public string? City { get; private set; }

    /// <summary>
    /// State or province of the branch.
    /// </summary>
    public string? State { get; private set; }

    /// <summary>
    /// Country of the branch location.
    /// </summary>
    public string? Country { get; private set; }

    /// <summary>
    /// Postal or ZIP code.
    /// </summary>
    public string? PostalCode { get; private set; }

    /// <summary>
    /// Contact phone number.
    /// </summary>
    public string? Phone { get; private set; }

    /// <summary>
    /// Contact email address.
    /// </summary>
    public string? Email { get; private set; }

    /// <summary>
    /// Branch type classification.
    /// </summary>
    public string BranchType { get; private set; } = TypeBranch;

    /// <summary>
    /// Reference to parent branch in hierarchy (null for head office).
    /// </summary>
    public Guid? ParentBranchId { get; private set; }

    /// <summary>
    /// Branch manager's name.
    /// </summary>
    public string? ManagerName { get; private set; }

    /// <summary>
    /// Branch manager's phone number.
    /// </summary>
    public string? ManagerPhone { get; private set; }

    /// <summary>
    /// Branch manager's email.
    /// </summary>
    public string? ManagerEmail { get; private set; }

    /// <summary>
    /// Date when the branch started operations.
    /// </summary>
    public DateOnly? OpeningDate { get; private set; }

    /// <summary>
    /// Date when the branch was closed (if applicable).
    /// </summary>
    public DateOnly? ClosingDate { get; private set; }

    /// <summary>
    /// Latitude coordinate for geolocation.
    /// </summary>
    public decimal? Latitude { get; private set; }

    /// <summary>
    /// Longitude coordinate for geolocation.
    /// </summary>
    public decimal? Longitude { get; private set; }

    /// <summary>
    /// Operating hours configuration (JSON format).
    /// </summary>
    public string? OperatingHours { get; private set; }

    /// <summary>
    /// Timezone of the branch.
    /// </summary>
    public string? Timezone { get; private set; }

    /// <summary>
    /// Whether the branch is currently active.
    /// </summary>
    public string Status { get; private set; } = StatusActive;

    /// <summary>
    /// Maximum cash holding limit for the branch vault.
    /// </summary>
    public decimal? CashHoldingLimit { get; private set; }

    // Navigation properties
    public Branch? ParentBranch { get; private set; }
    public ICollection<Branch> ChildBranches { get; private set; } = new List<Branch>();
    public ICollection<BranchTarget> Targets { get; private set; } = new List<BranchTarget>();
    public ICollection<CashVault> CashVaults { get; private set; } = new List<CashVault>();
    public ICollection<TellerSession> TellerSessions { get; private set; } = new List<TellerSession>();

    private Branch() { }

    /// <summary>
    /// Creates a new branch with the specified details.
    /// </summary>
    public static Branch Create(
        string code,
        string name,
        string branchType,
        Guid? parentBranchId = null,
        string? address = null,
        string? city = null,
        string? state = null,
        string? country = null,
        string? phone = null,
        string? email = null,
        DateOnly? openingDate = null)
    {
        var branch = new Branch
        {
            Code = code,
            BranchType = branchType,
            ParentBranchId = parentBranchId,
            Address = address,
            City = city,
            State = state,
            Country = country,
            Phone = phone,
            Email = email,
            OpeningDate = openingDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            Status = StatusActive
        };
        branch.Name = name;

        branch.QueueDomainEvent(new BranchCreated(branch));
        return branch;
    }

    /// <summary>
    /// Updates branch information.
    /// </summary>
    public Branch Update(
        string? name,
        string? address,
        string? city,
        string? state,
        string? country,
        string? postalCode,
        string? phone,
        string? email,
        string? managerName,
        string? managerPhone,
        string? managerEmail,
        decimal? latitude,
        decimal? longitude,
        string? operatingHours,
        string? timezone,
        decimal? cashHoldingLimit,
        string? notes)
    {
        bool hasChanges = false;

        if (name is not null && Name != name) { Name = name; hasChanges = true; }
        if (address is not null && Address != address) { Address = address; hasChanges = true; }
        if (city is not null && City != city) { City = city; hasChanges = true; }
        if (state is not null && State != state) { State = state; hasChanges = true; }
        if (country is not null && Country != country) { Country = country; hasChanges = true; }
        if (postalCode is not null && PostalCode != postalCode) { PostalCode = postalCode; hasChanges = true; }
        if (phone is not null && Phone != phone) { Phone = phone; hasChanges = true; }
        if (email is not null && Email != email) { Email = email; hasChanges = true; }
        if (managerName is not null && ManagerName != managerName) { ManagerName = managerName; hasChanges = true; }
        if (managerPhone is not null && ManagerPhone != managerPhone) { ManagerPhone = managerPhone; hasChanges = true; }
        if (managerEmail is not null && ManagerEmail != managerEmail) { ManagerEmail = managerEmail; hasChanges = true; }
        if (latitude.HasValue && Latitude != latitude) { Latitude = latitude; hasChanges = true; }
        if (longitude.HasValue && Longitude != longitude) { Longitude = longitude; hasChanges = true; }
        if (operatingHours is not null && OperatingHours != operatingHours) { OperatingHours = operatingHours; hasChanges = true; }
        if (timezone is not null && Timezone != timezone) { Timezone = timezone; hasChanges = true; }
        if (cashHoldingLimit.HasValue && CashHoldingLimit != cashHoldingLimit) { CashHoldingLimit = cashHoldingLimit; hasChanges = true; }
        if (notes is not null && Notes != notes) { Notes = notes; hasChanges = true; }

        if (hasChanges)
        {
            QueueDomainEvent(new BranchUpdated(this));
        }

        return this;
    }

    /// <summary>
    /// Assigns a manager to the branch.
    /// </summary>
    public void AssignManager(string managerName, string? managerPhone = null, string? managerEmail = null)
    {
        ManagerName = managerName;
        ManagerPhone = managerPhone;
        ManagerEmail = managerEmail;
        QueueDomainEvent(new BranchManagerAssigned(Id, managerName));
    }

    /// <summary>
    /// Activates the branch for operations.
    /// </summary>
    public void Activate()
    {
        if (Status == StatusActive) return;
        Status = StatusActive;
        QueueDomainEvent(new BranchActivated(Id));
    }

    /// <summary>
    /// Deactivates the branch temporarily.
    /// </summary>
    public void Deactivate()
    {
        if (Status == StatusInactive) return;
        Status = StatusInactive;
        QueueDomainEvent(new BranchDeactivated(Id));
    }

    /// <summary>
    /// Closes the branch permanently.
    /// </summary>
    public void Close(DateOnly? closingDate = null)
    {
        Status = StatusPermanentlyClosed;
        ClosingDate = closingDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
        QueueDomainEvent(new BranchClosed(Id, ClosingDate.Value));
    }
}
