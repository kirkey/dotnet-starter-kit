using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a staff member (employee) of the MFI (Microfinance Institution).
/// Manages employee information, role assignments, branch assignments, and work history.
/// </summary>
/// <remarks>
/// Use cases:
/// - Register and manage MFI employees across all roles.
/// - Assign staff to branches and define reporting hierarchies.
/// - Track loan officer portfolios and performance metrics.
/// - Manage teller sessions and daily cash handling.
/// - Link staff to system user accounts for authentication.
/// - Maintain HR records including employment dates, salary, and benefits.
/// 
/// Default values and constraints:
/// - EmployeeNumber: required unique identifier, max 32 characters (example: "EMP-2024-001")
/// - FirstName: required, max 64 characters
/// - LastName: required, max 64 characters
/// - Email: required, max 128 characters, must be valid email format
/// - Phone: optional, max 32 characters
/// - Designation: required, one of LoanOfficer, Teller, BranchManager, CollectionOfficer, Accountant, Compliance, Auditor
/// - Status: "Active" by default (Active, OnLeave, Suspended, Terminated, Resigned, Retired)
/// - BranchId: required, must reference an active branch
/// - HireDate: required, date of employment start
/// 
/// Business rules:
/// - EmployeeNumber must be unique within the system.
/// - Cannot delete staff with active loan portfolios or pending transactions.
/// - Loan officers must be assigned to specific members or groups.
/// - Tellers must have daily session reconciliation before shift end.
/// - Status changes require appropriate approvals and documentation.
/// - Terminated or resigned staff accounts should be deactivated.
/// - Salary and benefits information requires HR permissions.
/// </remarks>
/// <seealso cref="Branch"/>
/// <seealso cref="LoanOfficerAssignment"/>
/// <seealso cref="LoanOfficerTarget"/>
/// <seealso cref="TellerSession"/>
/// <seealso cref="StaffTraining"/>
/// <seealso cref="ApprovalRequest"/>
public sealed class Staff : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Maximum length constants for string properties.
    /// </summary>
    public static class MaxLengths
    {
        public const int EmployeeNumber = 32;
        public const int FirstName = 64;
        public const int LastName = 64;
        public const int MiddleName = 64;
        public const int Email = 128;
        public const int Phone = 32;
        public const int AlternatePhone = 32;
        public const int Address = 512;
        public const int NationalId = 64;
        public const int Department = 64;
        public const int JobTitle = 128;
        public const int Designation = 64;
        public const int ReportingTo = 128;
        public const int EmergencyContactName = 128;
        public const int EmergencyContactPhone = 32;
        public const int BankAccountNumber = 64;
        public const int BankName = 128;
        public const int Notes = 4096;
    }

    /// <summary>
    /// Staff type classification.
    /// </summary>
    public const string TypeFullTime = "FullTime";
    public const string TypePartTime = "PartTime";
    public const string TypeContract = "Contract";
    public const string TypeIntern = "Intern";
    public const string TypeVolunteer = "Volunteer";

    /// <summary>
    /// Staff role categories.
    /// </summary>
    public const string RoleLoanOfficer = "LoanOfficer";
    public const string RoleTeller = "Teller";
    public const string RoleBranchManager = "BranchManager";
    public const string RoleAccountant = "Accountant";
    public const string RoleCollectionOfficer = "CollectionOfficer";
    public const string RoleCustomerService = "CustomerService";
    public const string RoleITSupport = "ITSupport";
    public const string RoleHR = "HR";
    public const string RoleCompliance = "Compliance";
    public const string RoleAuditor = "Auditor";
    public const string RoleExecutive = "Executive";

    /// <summary>
    /// Employment status.
    /// </summary>
    public const string StatusActive = "Active";
    public const string StatusOnLeave = "OnLeave";
    public const string StatusSuspended = "Suspended";
    public const string StatusTerminated = "Terminated";
    public const string StatusResigned = "Resigned";
    public const string StatusRetired = "Retired";

    /// <summary>
    /// Unique employee number.
    /// </summary>
    public string EmployeeNumber { get; private set; } = string.Empty;

    /// <summary>
    /// User account ID (if linked to system user).
    /// </summary>
    public Guid? UserId { get; private set; }

    /// <summary>
    /// First name of the staff member.
    /// </summary>
    public string FirstName { get; private set; } = string.Empty;

    /// <summary>
    /// Last name of the staff member.
    /// </summary>
    public string LastName { get; private set; } = string.Empty;

    /// <summary>
    /// Middle name of the staff member.
    /// </summary>
    public string? MiddleName { get; private set; }

    /// <summary>
    /// Full name computed from first, middle, and last names.
    /// </summary>
    public string FullName => string.IsNullOrEmpty(MiddleName)
        ? $"{FirstName} {LastName}"
        : $"{FirstName} {MiddleName} {LastName}";

    /// <summary>
    /// Email address.
    /// </summary>
    public string Email { get; private set; } = string.Empty;

    /// <summary>
    /// Primary phone number.
    /// </summary>
    public string? Phone { get; private set; }

    /// <summary>
    /// Alternate phone number.
    /// </summary>
    public string? AlternatePhone { get; private set; }

    /// <summary>
    /// Date of birth.
    /// </summary>
    public DateOnly? DateOfBirth { get; private set; }

    /// <summary>
    /// Gender.
    /// </summary>
    public string? Gender { get; private set; }

    /// <summary>
    /// National ID or government ID number.
    /// </summary>
    public string? NationalId { get; private set; }

    /// <summary>
    /// Residential address.
    /// </summary>
    public string? Address { get; private set; }

    /// <summary>
    /// Primary branch assignment.
    /// </summary>
    public Guid? BranchId { get; private set; }

    /// <summary>
    /// Department within the organization.
    /// </summary>
    public string? Department { get; private set; }

    /// <summary>
    /// Job title.
    /// </summary>
    public string JobTitle { get; private set; } = string.Empty;

    /// <summary>
    /// Designation or rank.
    /// </summary>
    public string? Designation { get; private set; }

    /// <summary>
    /// Primary role of the staff member.
    /// </summary>
    public string Role { get; private set; } = RoleLoanOfficer;

    /// <summary>
    /// Employment type (Full-time, Part-time, etc.).
    /// </summary>
    public string EmploymentType { get; private set; } = TypeFullTime;

    /// <summary>
    /// Date when the staff member joined.
    /// </summary>
    public DateOnly JoiningDate { get; private set; }

    /// <summary>
    /// End date of probation period.
    /// </summary>
    public DateOnly? ProbationEndDate { get; private set; }

    /// <summary>
    /// Date of confirmation after probation.
    /// </summary>
    public DateOnly? ConfirmationDate { get; private set; }

    /// <summary>
    /// Date of termination/resignation (if applicable).
    /// </summary>
    public DateOnly? TerminationDate { get; private set; }

    /// <summary>
    /// ID of the reporting manager.
    /// </summary>
    public Guid? ReportingManagerId { get; private set; }

    /// <summary>
    /// Name of the reporting manager.
    /// </summary>
    public string? ReportingTo { get; private set; }

    /// <summary>
    /// Basic salary amount.
    /// </summary>
    public decimal? BasicSalary { get; private set; }

    /// <summary>
    /// Bank account number for salary.
    /// </summary>
    public string? BankAccountNumber { get; private set; }

    /// <summary>
    /// Bank name.
    /// </summary>
    public string? BankName { get; private set; }

    /// <summary>
    /// Emergency contact name.
    /// </summary>
    public string? EmergencyContactName { get; private set; }

    /// <summary>
    /// Emergency contact phone.
    /// </summary>
    public string? EmergencyContactPhone { get; private set; }

    /// <summary>
    /// Path to profile photo.
    /// </summary>
    public string? PhotoUrl { get; private set; }

    /// <summary>
    /// Current employment status.
    /// </summary>
    public string Status { get; private set; } = StatusActive;

    /// <summary>
    /// Whether the staff can approve loans.
    /// </summary>
    public bool CanApproveLoan { get; private set; }

    /// <summary>
    /// Maximum loan amount the staff can approve.
    /// </summary>
    public decimal? LoanApprovalLimit { get; private set; }

    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; private set; }

    // Navigation properties
    public Branch? Branch { get; private set; }
    public Staff? ReportingManager { get; private set; }
    public ICollection<Staff> DirectReports { get; private set; } = new List<Staff>();
    public ICollection<LoanOfficerAssignment> LoanOfficerAssignments { get; private set; } = new List<LoanOfficerAssignment>();
    public ICollection<LoanOfficerTarget> LoanOfficerTargets { get; private set; } = new List<LoanOfficerTarget>();
    public ICollection<StaffTraining> Trainings { get; private set; } = new List<StaffTraining>();

    private Staff() { }

    /// <summary>
    /// Creates a new staff member.
    /// </summary>
    public static Staff Create(
        string employeeNumber,
        string firstName,
        string lastName,
        string email,
        string jobTitle,
        string role,
        DateOnly joiningDate,
        string employmentType = TypeFullTime,
        Guid? branchId = null,
        string? department = null,
        Guid? userId = null)
    {
        var staff = new Staff
        {
            EmployeeNumber = employeeNumber,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            JobTitle = jobTitle,
            Role = role,
            JoiningDate = joiningDate,
            EmploymentType = employmentType,
            BranchId = branchId,
            Department = department,
            UserId = userId,
            Status = StatusActive
        };

        staff.QueueDomainEvent(new StaffCreated(staff));
        return staff;
    }

    /// <summary>
    /// Updates staff personal information.
    /// </summary>
    public Staff Update(
        string? firstName,
        string? lastName,
        string? middleName,
        string? phone,
        string? alternatePhone,
        DateOnly? dateOfBirth,
        string? gender,
        string? nationalId,
        string? address,
        string? emergencyContactName,
        string? emergencyContactPhone,
        string? photoUrl,
        string? notes)
    {
        if (firstName is not null) FirstName = firstName;
        if (lastName is not null) LastName = lastName;
        if (middleName is not null) MiddleName = middleName;
        if (phone is not null) Phone = phone;
        if (alternatePhone is not null) AlternatePhone = alternatePhone;
        if (dateOfBirth.HasValue) DateOfBirth = dateOfBirth.Value;
        if (gender is not null) Gender = gender;
        if (nationalId is not null) NationalId = nationalId;
        if (address is not null) Address = address;
        if (emergencyContactName is not null) EmergencyContactName = emergencyContactName;
        if (emergencyContactPhone is not null) EmergencyContactPhone = emergencyContactPhone;
        if (photoUrl is not null) PhotoUrl = photoUrl;
        if (notes is not null) Notes = notes;

        QueueDomainEvent(new StaffUpdated(this));
        return this;
    }

    /// <summary>
    /// Assigns the staff to a branch.
    /// </summary>
    public void AssignToBranch(Guid branchId)
    {
        var previousBranchId = BranchId;
        BranchId = branchId;
        QueueDomainEvent(new StaffBranchAssigned(Id, previousBranchId, branchId));
    }

    /// <summary>
    /// Promotes or changes the job role.
    /// </summary>
    public void ChangeRole(string newJobTitle, string? newRole = null, string? newDesignation = null, decimal? newSalary = null)
    {
        var previousTitle = JobTitle;
        var previousRole = Role;

        JobTitle = newJobTitle;
        if (newRole is not null) Role = newRole;
        if (newDesignation is not null) Designation = newDesignation;
        if (newSalary.HasValue) BasicSalary = newSalary.Value;

        QueueDomainEvent(new StaffRoleChanged(Id, previousTitle, newJobTitle, previousRole, Role));
    }

    /// <summary>
    /// Sets the reporting manager.
    /// </summary>
    public void SetReportingManager(Guid managerId, string managerName)
    {
        ReportingManagerId = managerId;
        ReportingTo = managerName;
        QueueDomainEvent(new StaffReportingManagerSet(Id, managerId, managerName));
    }

    /// <summary>
    /// Sets loan approval authority.
    /// </summary>
    public void SetLoanApprovalAuthority(bool canApprove, decimal? approvalLimit = null)
    {
        CanApproveLoan = canApprove;
        LoanApprovalLimit = approvalLimit;
        QueueDomainEvent(new StaffLoanApprovalAuthoritySet(Id, canApprove, approvalLimit));
    }

    /// <summary>
    /// Confirms the staff after probation.
    /// </summary>
    public void Confirm(DateOnly confirmationDate)
    {
        ConfirmationDate = confirmationDate;
        QueueDomainEvent(new StaffConfirmed(Id, confirmationDate));
    }

    /// <summary>
    /// Places the staff on leave.
    /// </summary>
    public void PlaceOnLeave()
    {
        Status = StatusOnLeave;
        QueueDomainEvent(new StaffPlacedOnLeave(Id));
    }

    /// <summary>
    /// Suspends the staff member.
    /// </summary>
    public void Suspend(string? reason = null)
    {
        Status = StatusSuspended;
        if (reason is not null) Notes = reason;
        QueueDomainEvent(new StaffSuspended(Id, reason));
    }

    /// <summary>
    /// Reinstates a suspended staff member.
    /// </summary>
    public void Reinstate()
    {
        Status = StatusActive;
        QueueDomainEvent(new StaffReinstated(Id));
    }

    /// <summary>
    /// Terminates employment.
    /// </summary>
    public void Terminate(DateOnly terminationDate, string? reason = null)
    {
        Status = StatusTerminated;
        TerminationDate = terminationDate;
        if (reason is not null) Notes = reason;
        QueueDomainEvent(new StaffTerminated(Id, terminationDate, reason));
    }

    /// <summary>
    /// Records resignation.
    /// </summary>
    public void Resign(DateOnly lastWorkingDate)
    {
        Status = StatusResigned;
        TerminationDate = lastWorkingDate;
        QueueDomainEvent(new StaffResigned(Id, lastWorkingDate));
    }
}
