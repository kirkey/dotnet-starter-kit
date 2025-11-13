using FSH.Starter.WebApi.HumanResources.Domain.Events;

namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents an employee in the organization.
/// Tracks basic employee information, employment status, and relationships to organizational units and designations.
/// </summary>
/// <remarks>
/// Key Design Points:
/// - Employee belongs to an organizational unit
/// - Employee can have multiple designation assignments (primary and acting)
/// - Employment status tracks hiring, termination, and leave status
/// - Supports full employee lifecycle management
/// 
/// Example:
/// - Employee John Doe
///   - Employee Number: EMP-001
///   - Organizational Unit: Area 1 (Department)
///   - Primary Designation: Supervisor
///   - Status: Active
///   - Can have Acting As: Senior Manager (temporary)
/// </remarks>
public class Employee : AuditableEntity, IAggregateRoot
{
    private Employee() { }

    private Employee(
        DefaultIdType id,
        string employeeNumber,
        string firstName,
        string lastName,
        DefaultIdType organizationalUnitId,
        string? middleName = null,
        string? email = null,
        string? phoneNumber = null)
    {
        Id = id;
        EmployeeNumber = employeeNumber;
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        OrganizationalUnitId = organizationalUnitId;
        Email = email;
        PhoneNumber = phoneNumber;
        Status = EmploymentStatus.Active;
        IsActive = true;

        QueueDomainEvent(new EmployeeCreated { Employee = this });
    }

    /// <summary>
    /// Unique employee number/ID within the organization.
    /// Example: "EMP-001", "HR-2025-001"
    /// </summary>
    public string EmployeeNumber { get; private set; } = default!;

    /// <summary>
    /// Employee's first name.
    /// </summary>
    public string FirstName { get; private set; } = default!;

    /// <summary>
    /// Employee's middle name (optional).
    /// </summary>
    public string? MiddleName { get; private set; }

    /// <summary>
    /// Employee's last name.
    /// </summary>
    public string LastName { get; private set; } = default!;

    /// <summary>
    /// Full name of the employee (read-only, computed).
    /// </summary>
    public string FullName => $"{FirstName} {MiddleName} {LastName}".Trim();

    /// <summary>
    /// The organizational unit (Department/Area) this employee belongs to.
    /// </summary>
    public DefaultIdType OrganizationalUnitId { get; private set; }
    public OrganizationalUnit OrganizationalUnit { get; private set; } = default!;

    /// <summary>
    /// Employee's email address.
    /// </summary>
    public string? Email { get; private set; }

    /// <summary>
    /// Employee's phone number.
    /// </summary>
    public string? PhoneNumber { get; private set; }

    /// <summary>
    /// Date the employee was hired.
    /// </summary>
    public DateTime? HireDate { get; private set; }

    /// <summary>
    /// Employment status (Active, OnLeave, Terminated, etc.).
    /// </summary>
    public string Status { get; private set; } = EmploymentStatus.Active;

    /// <summary>
    /// Date employee was terminated (if applicable).
    /// </summary>
    public DateTime? TerminationDate { get; private set; }

    /// <summary>
    /// Reason for termination if applicable.
    /// </summary>
    public string? TerminationReason { get; private set; }

    /// <summary>
    /// Whether this employee record is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Collection of designation assignments for this employee.
    /// </summary>
    public ICollection<DesignationAssignment> DesignationAssignments { get; private set; } = new List<DesignationAssignment>();

    /// <summary>
    /// Collection of contacts for this employee (emergency, references, family).
    /// </summary>
    public ICollection<EmployeeContact> Contacts { get; private set; } = new List<EmployeeContact>();

    /// <summary>
    /// Collection of dependents for this employee.
    /// </summary>
    public ICollection<EmployeeDependent> Dependents { get; private set; } = new List<EmployeeDependent>();

    /// <summary>
    /// Collection of documents for this employee.
    /// </summary>
    public ICollection<EmployeeDocument> Documents { get; private set; } = new List<EmployeeDocument>();

    /// <summary>
    /// Collection of attendance records for this employee.
    /// </summary>
    public ICollection<Attendance> AttendanceRecords { get; private set; } = new List<Attendance>();

    /// <summary>
    /// Collection of timesheets for this employee.
    /// </summary>
    public ICollection<Timesheet> Timesheets { get; private set; } = new List<Timesheet>();

    /// <summary>
    /// Collection of shift assignments for this employee.
    /// </summary>
    public ICollection<ShiftAssignment> ShiftAssignments { get; private set; } = new List<ShiftAssignment>();

    /// <summary>
    /// Creates a new employee record.
    /// </summary>
    public static Employee Create(
        string employeeNumber,
        string firstName,
        string lastName,
        DefaultIdType organizationalUnitId,
        string? middleName = null,
        string? email = null,
        string? phoneNumber = null)
    {
        var employee = new Employee(
            DefaultIdType.NewGuid(),
            employeeNumber,
            firstName,
            lastName,
            organizationalUnitId,
            middleName,
            email,
            phoneNumber);

        return employee;
    }

    /// <summary>
    /// Sets the hire date for this employee.
    /// </summary>
    public Employee SetHireDate(DateTime hireDate)
    {
        HireDate = hireDate;
        Status = EmploymentStatus.Active;
        QueueDomainEvent(new EmployeeHired { EmployeeId = Id, HireDate = hireDate });
        return this;
    }

    /// <summary>
    /// Updates employee contact information.
    /// </summary>
    public Employee UpdateContactInfo(string? email = null, string? phoneNumber = null)
    {
        bool updated = false;

        if (!string.IsNullOrWhiteSpace(email) && Email != email)
        {
            Email = email;
            updated = true;
        }

        if (!string.IsNullOrWhiteSpace(phoneNumber) && PhoneNumber != phoneNumber)
        {
            PhoneNumber = phoneNumber;
            updated = true;
        }

        if (updated)
        {
            QueueDomainEvent(new EmployeeContactInfoUpdated { Employee = this });
        }

        return this;
    }

    /// <summary>
    /// Updates employee's organizational unit.
    /// </summary>
    public Employee UpdateOrganizationalUnit(DefaultIdType organizationalUnitId)
    {
        if (OrganizationalUnitId != organizationalUnitId)
        {
            var previousUnitId = OrganizationalUnitId;
            OrganizationalUnitId = organizationalUnitId;
            QueueDomainEvent(new EmployeeTransferred { EmployeeId = Id, FromUnitId = previousUnitId, ToUnitId = organizationalUnitId });
        }
        return this;
    }

    /// <summary>
    /// Marks employee as on leave.
    /// </summary>
    public Employee MarkOnLeave()
    {
        if (Status != EmploymentStatus.OnLeave)
        {
            Status = EmploymentStatus.OnLeave;
            QueueDomainEvent(new EmployeeOnLeave { EmployeeId = Id });
        }
        return this;
    }

    /// <summary>
    /// Returns employee from leave.
    /// </summary>
    public Employee ReturnFromLeave()
    {
        if (Status == EmploymentStatus.OnLeave)
        {
            Status = EmploymentStatus.Active;
            QueueDomainEvent(new EmployeeReturnedFromLeave { EmployeeId = Id });
        }
        return this;
    }

    /// <summary>
    /// Terminates the employee.
    /// </summary>
    public Employee Terminate(DateTime terminationDate, string? reason = null)
    {
        if (Status != EmploymentStatus.Terminated)
        {
            TerminationDate = terminationDate;
            TerminationReason = reason;
            Status = EmploymentStatus.Terminated;
            IsActive = false;
            QueueDomainEvent(new EmployeeTerminated { EmployeeId = Id, TerminationDate = terminationDate, Reason = reason });
        }
        return this;
    }

    /// <summary>
    /// Gets the current active designation assignment.
    /// </summary>
    public DesignationAssignment? GetCurrentDesignation()
    {
        return DesignationAssignments
            .FirstOrDefault(d => d.IsPlantilla && d.IsCurrentlyEffective());
    }

    /// <summary>
    /// Gets all current acting as designations.
    /// </summary>
    public IEnumerable<DesignationAssignment> GetCurrentActingDesignations()
    {
        return DesignationAssignments
            .Where(d => d.IsActingAs && d.IsCurrentlyEffective());
    }
}

/// <summary>
/// Employment status constants.
/// </summary>
public static class EmploymentStatus
{
    public const string Active = "Active";
    public const string OnLeave = "OnLeave";
    public const string Suspended = "Suspended";
    public const string Terminated = "Terminated";
    public const string Probationary = "Probationary";
}

