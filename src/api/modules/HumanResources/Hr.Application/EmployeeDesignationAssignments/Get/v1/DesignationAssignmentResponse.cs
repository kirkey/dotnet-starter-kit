namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDesignationAssignments.Get.v1;

/// <summary>
/// Response for designation assignment details.
/// </summary>
public sealed record DesignationAssignmentResponse
{
    public DefaultIdType Id { get; init; }
    public DefaultIdType EmployeeId { get; init; }
    public string EmployeeNumber { get; init; } = default!;
    public string EmployeeName { get; init; } = default!;
    public DefaultIdType DesignationId { get; init; }
    public string DesignationTitle { get; init; } = default!;
    public DateTime EffectiveDate { get; init; }
    public DateTime? EndDate { get; init; }
    public bool IsPlantilla { get; init; }
    public bool IsActingAs { get; init; }
    public decimal? AdjustedSalary { get; init; }
    public string? Reason { get; init; }
    public int TenureMonths { get; init; }
    public string TenureDisplay { get; init; } = default!;
    public bool IsCurrentlyActive { get; init; }
}

