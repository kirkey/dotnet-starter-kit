namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Benefit type constants.
/// </summary>
public static class BenefitType
{
    /// <summary>Health insurance benefit.</summary>
    public const string Health = "Health";

    /// <summary>Dental insurance benefit.</summary>
    public const string Dental = "Dental";

    /// <summary>Vision insurance benefit.</summary>
    public const string Vision = "Vision";

    /// <summary>Retirement/pension benefit.</summary>
    public const string Retirement = "Retirement";

    /// <summary>Life insurance benefit.</summary>
    public const string LifeInsurance = "LifeInsurance";

    /// <summary>Disability insurance benefit.</summary>
    public const string Disability = "Disability";

    /// <summary>Wellness program benefit.</summary>
    public const string Wellness = "Wellness";
}

/// <summary>
/// Coverage level constants for benefit enrollments.
/// </summary>
public static class CoverageLevel
{
    /// <summary>Individual coverage.</summary>
    public const string Individual = "Individual";

    /// <summary>Employee plus spouse coverage.</summary>
    public const string EmployeePlusSpouse = "Employee_Plus_Spouse";

    /// <summary>Employee plus children coverage.</summary>
    public const string EmployeePlusChildren = "Employee_Plus_Children";

    /// <summary>Family coverage (Employee + all dependents).</summary>
    public const string Family = "Family";
}

