namespace FSH.Starter.Blazor.Client.Pages.Accounting.DepreciationMethods;

/// <summary>
/// ViewModel used for creating or editing depreciation methods.
/// </summary>
public sealed class DepreciationMethodViewModel
{
    public DefaultIdType? Id { get; set; }
    public string? Name { get; set; }
    public string? MethodCode { get; set; }
    public string? Description { get; set; }
    public string? Formula { get; set; }
    public string? Notes { get; set; }
    public string? Remarks { get; set; }
    public bool IsActive { get; set; } = true;
    public string Status { get; set; } = "Active";
}

