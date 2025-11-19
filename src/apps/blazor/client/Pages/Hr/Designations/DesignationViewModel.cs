using FSH.Starter.Blazor.Infrastructure.Api;

namespace FSH.Starter.Blazor.Client.Pages.Hr.Designations;

/// <summary>
/// View model for designation CRUD operations.
/// Extends the update command with additional display properties.
/// </summary>
public class DesignationViewModel : UpdateDesignationCommand
{
    /// <summary>
    /// The unique code for the designation.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// The area/region where this designation applies (Metro Manila, Visayas, Mindanao, Luzon, National).
    /// </summary>
    public string? Area { get; set; }

    /// <summary>
    /// The salary grade for compensation structuring.
    /// </summary>
    public string? SalaryGrade { get; set; }

    /// <summary>
    /// The minimum salary for this designation.
    /// </summary>
    public decimal? MinimumSalary { get; set; }

    /// <summary>
    /// The maximum salary for this designation.
    /// </summary>
    public decimal? MaximumSalary { get; set; }

    /// <summary>
    /// The midpoint salary (average of min and max).
    /// </summary>
    public decimal? MidpointSalary
    {
        get
        {
            if (MinimumSalary.HasValue && MaximumSalary.HasValue)
            {
                return (MinimumSalary.Value + MaximumSalary.Value) / 2;
            }
            return null;
        }
    }

    /// <summary>
    /// Whether the designation is active and available for assignment.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Whether this designation is for managerial positions.
    /// </summary>
    public bool IsManagerial { get; set; }
}

