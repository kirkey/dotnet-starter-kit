namespace FSH.Starter.Blazor.Client.Models.NavigationMenu;

public class MenuSectionSubItemModel
{
    public string Title { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string Href { get; set; } = string.Empty;
    public string? Target { get; set; }
    public bool Disabled { get; set; }
    public PageStatus PageStatus { get; set; } = PageStatus.None;
    public string[]? Roles { get; set; }
    public string? Permission { get; set; }
    public string? Action { get; set; }
    public string? Resource { get; set; }
}
