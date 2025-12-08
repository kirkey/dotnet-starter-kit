namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.MfiConfigurations;

/// <summary>
/// ViewModel for MFI Configuration add/edit operations.
/// </summary>
public class MfiConfigurationViewModel
{
    public DefaultIdType Id { get; set; }
    public string? Key { get; set; }
    public string? Value { get; set; }
    public string? Category { get; set; }
    public string? DataType { get; set; }
    public string? Description { get; set; }
    public bool IsEncrypted { get; set; }
    public bool IsEditable { get; set; } = true;
    public bool RequiresRestart { get; set; }
    public string? DefaultValue { get; set; }
    public string? ValidationRules { get; set; }
    public DefaultIdType? BranchId { get; set; }
    public int DisplayOrder { get; set; }
}
