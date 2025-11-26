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
    // Domain Constants - Binary Limits (Powers of 2)
    /// <summary>
    /// Maximum length for the employee number field. (50)
    /// </summary>
    public const int EmployeeNumberMaxLength = 50;

    /// <summary>
    /// Maximum length for the first name field. (100)
    /// </summary>
    public const int FirstNameMaxLength = 100;

    /// <summary>
    /// Maximum length for the middle name field. (100)
    /// </summary>
    public const int MiddleNameMaxLength = 100;

    /// <summary>
    /// Maximum length for the last name field. (100)
    /// </summary>
    public const int LastNameMaxLength = 100;

    /// <summary>
    /// Maximum length for the email field. (2^8 = 256)
    /// </summary>
    public const int EmailMaxLength = 256;

    /// <summary>
    /// Maximum length for the phone number field. (20)
    /// </summary>
    public const int PhoneMaxLength = 20;

    /// <summary>
    /// Maximum length for TIN (Tax Identification Number). (20)
    /// Format: XXX-XXX-XXX-XXX
    /// </summary>
    public const int TinMaxLength = 20;

    /// <summary>
    /// Maximum length for SSS Number. (20)
    /// Format: XX-XXXXXXX-X
    /// </summary>
    public const int SssNumberMaxLength = 20;

    /// <summary>
    /// Maximum length for PhilHealth Number. (20)
    /// Format: XX-XXXXXXXXX-X
    /// </summary>
    public const int PhilHealthNumberMaxLength = 20;

    /// <summary>
    /// Maximum length for Pag-IBIG Number. (20)
    /// Format: XXXX-XXXX-XXXX
    /// </summary>
    public const int PagIbigNumberMaxLength = 20;

    /// <summary>
    /// Maximum length for PWD ID Number. (50)
    /// </summary>
    public const int PwdIdNumberMaxLength = 50;

    /// <summary>
    /// Maximum length for Solo Parent ID Number. (50)
    /// </summary>
    public const int SoloParentIdNumberMaxLength = 50;

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
    /// Date the employee was born (for age verification and benefits).
    /// Required for Philippine government benefits (SSS, PhilHealth, Pag-IBIG).
    /// </summary>
    public DateTime? BirthDate { get; private set; }

    /// <summary>
    /// Gender of the employee (Male, Female).
    /// Required for maternity/paternity leave eligibility per Philippine Labor Code.
    /// </summary>
    public string? Gender { get; private set; }

    /// <summary>
    /// Civil status (Single, Married, Widowed, Separated, etc.).
    /// Affects tax computations and dependent benefits.
    /// </summary>
    public string? CivilStatus { get; private set; }

    /// <summary>
    /// Tax Identification Number (TIN) issued by BIR.
    /// Mandatory for all employees in the Philippines.
    /// Format: XXX-XXX-XXX-XXX
    /// </summary>
    public string? Tin { get; private set; }

    /// <summary>
    /// SSS (Social Security System) Number.
    /// Mandatory for all employees earning ₱1,000/month or more.
    /// Format: XX-XXXXXXX-X
    /// </summary>
    public string? SssNumber { get; private set; }

    /// <summary>
    /// PhilHealth Number (Philippine Health Insurance).
    /// Mandatory for all employees.
    /// Format: XX-XXXXXXXXX-X
    /// </summary>
    public string? PhilHealthNumber { get; private set; }

    /// <summary>
    /// Pag-IBIG (HDMF) Number.
    /// Mandatory for all employees earning ₱1,500/month or more.
    /// Format: XXXX-XXXX-XXXX
    /// </summary>
    public string? PagIbigNumber { get; private set; }

    /// <summary>
    /// Employment classification per Labor Code Article 280.
    /// Values: Regular, Probationary, Casual, ProjectBased, Seasonal, Contractual.
    /// Determines benefits eligibility and security of tenure.
    /// </summary>
    public string EmploymentClassification { get; private set; } = "Regular";

    /// <summary>
    /// Date employee was regularized (if applicable).
    /// Typically after 6 months probation for general employees.
    /// </summary>
    public DateTime? RegularizationDate { get; private set; }

    /// <summary>
    /// Basic monthly salary in Philippine Peso (₱).
    /// Used for 13th month pay, separation pay, and mandatory deductions.
    /// </summary>
    public decimal? BasicMonthlySalary { get; private set; }

    /// <summary>
    /// Date employee was terminated (if applicable).
    /// </summary>
    public DateTime? TerminationDate { get; private set; }

    /// <summary>
    /// Reason for termination per Labor Code (if applicable).
    /// Examples: MisconductJustCause, Redundancy, ResignationVoluntary, Retirement, etc.
    /// </summary>
    public string? TerminationReason { get; private set; }

    /// <summary>
    /// Termination mode: ByEmployer, ByEmployee, MutualConsent, ByOperationOfLaw.
    /// </summary>
    public string? TerminationMode { get; private set; }

    /// <summary>
    /// Separation pay basis if terminated.
    /// Examples: HalfMonthPerYear, OneMonthPerYear, None, CustomAmount.
    /// </summary>
    public string? SeparationPayBasis { get; private set; }

    /// <summary>
    /// Computed separation pay amount in PHP.
    /// </summary>
    public decimal? SeparationPayAmount { get; private set; }

    /// <summary>
    /// Whether this employee has PWD (Persons with Disabilities) status.
    /// Entitled to special benefits per RA 7277.
    /// </summary>
    public bool IsPwd { get; private set; }

    /// <summary>
    /// PWD ID Number if applicable.
    /// </summary>
    public string? PwdIdNumber { get; private set; }

    /// <summary>
    /// Whether employee is a solo parent per RA 7305.
    /// Entitled to 5 days solo parent leave annually.
    /// </summary>
    public bool IsSoloParent { get; private set; }

    /// <summary>
    /// Solo Parent ID Number if applicable (from DSWD).
    /// </summary>
    public string? SoloParentIdNumber { get; private set; }

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
    /// Collection of leave balances for this employee.
    /// </summary>
    public ICollection<LeaveBalance> LeaveBalances { get; private set; } = new List<LeaveBalance>();

    /// <summary>
    /// Collection of leave requests from this employee.
    /// </summary>
    public ICollection<LeaveRequest> LeaveRequests { get; private set; } = new List<LeaveRequest>();

    /// <summary>
    /// Collection of payroll lines for this employee.
    /// </summary>
    public ICollection<PayrollLine> PayrollLines { get; private set; } = new List<PayrollLine>();

    /// <summary>
    /// Collection of education records for this employee.
    /// </summary>
    public ICollection<EmployeeEducation> EducationRecords { get; private set; } = new List<EmployeeEducation>();

    /// <summary>
    /// Collection of benefit enrollments for this employee.
    /// </summary>
    public ICollection<BenefitEnrollment> BenefitEnrollments { get; private set; } = new List<BenefitEnrollment>();
    
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
    /// Terminates the employee per Philippine Labor Code.
    /// </summary>
    public Employee Terminate(
        DateTime terminationDate,
        string terminationReason,
        string terminationMode,
        string? separationPayBasis = null,
        decimal? separationPayAmount = null)
    {
        if (Status != EmploymentStatus.Terminated)
        {
            TerminationDate = terminationDate;
            TerminationReason = terminationReason;
            TerminationMode = terminationMode;
            SeparationPayBasis = separationPayBasis;
            SeparationPayAmount = separationPayAmount;
            Status = EmploymentStatus.Terminated;
            IsActive = false;
            QueueDomainEvent(new EmployeeTerminated { EmployeeId = Id, TerminationDate = terminationDate, Reason = terminationReason });
        }
        return this;
    }

    /// <summary>
    /// Sets employee government IDs (TIN, SSS, PhilHealth, Pag-IBIG).
    /// </summary>
    public Employee SetGovernmentIds(
        string? tin = null,
        string? sssNumber = null,
        string? philHealthNumber = null,
        string? pagIbigNumber = null)
    {
        if (!string.IsNullOrWhiteSpace(tin)) Tin = tin;
        if (!string.IsNullOrWhiteSpace(sssNumber)) SssNumber = sssNumber;
        if (!string.IsNullOrWhiteSpace(philHealthNumber)) PhilHealthNumber = philHealthNumber;
        if (!string.IsNullOrWhiteSpace(pagIbigNumber)) PagIbigNumber = pagIbigNumber;
        return this;
    }

    /// <summary>
    /// Sets employee personal information.
    /// </summary>
    public Employee SetPersonalInfo(
        DateTime? birthDate = null,
        string? gender = null,
        string? civilStatus = null)
    {
        if (birthDate.HasValue) BirthDate = birthDate;
        if (!string.IsNullOrWhiteSpace(gender)) Gender = gender;
        if (!string.IsNullOrWhiteSpace(civilStatus)) CivilStatus = civilStatus;
        return this;
    }

    /// <summary>
    /// Sets employee classification per Labor Code Article 280.
    /// </summary>
    public Employee SetEmploymentClassification(string classification)
    {
        EmploymentClassification = classification;
        return this;
    }

    /// <summary>
    /// Regularizes employee (from Probationary to Regular).
    /// </summary>
    public Employee Regularize(DateTime regularizationDate)
    {
        EmploymentClassification = "Regular";
        RegularizationDate = regularizationDate;
        Status = EmploymentStatus.Active;
        return this;
    }

    /// <summary>
    /// Sets employee basic monthly salary.
    /// </summary>
    public Employee SetBasicSalary(decimal basicMonthlySalary)
    {
        BasicMonthlySalary = basicMonthlySalary;
        return this;
    }

    /// <summary>
    /// Sets PWD status and ID.
    /// </summary>
    public Employee SetPwdStatus(bool isPwd, string? pwdIdNumber = null)
    {
        IsPwd = isPwd;
        PwdIdNumber = pwdIdNumber;
        return this;
    }

    /// <summary>
    /// Sets solo parent status and ID.
    /// </summary>
    public Employee SetSoloParentStatus(bool isSoloParent, string? soloParentIdNumber = null)
    {
        IsSoloParent = isSoloParent;
        SoloParentIdNumber = soloParentIdNumber;
        return this;
    }

    /// <summary>
    /// Calculates separation pay based on years of service and separation pay basis.
    /// </summary>
    public decimal CalculateSeparationPay()
    {
        if (!HireDate.HasValue || !TerminationDate.HasValue || !BasicMonthlySalary.HasValue)
            return 0m;

        var yearsOfService = (TerminationDate.Value - HireDate.Value).TotalDays / 365.25;

        return SeparationPayBasis switch
        {
            "HalfMonthPerYear" => BasicMonthlySalary.Value * 0.5m * (decimal)yearsOfService,
            "OneMonthPerYear" => BasicMonthlySalary.Value * (decimal)yearsOfService,
            "CustomAmount" => SeparationPayAmount ?? 0m,
            _ => 0m
        };
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

