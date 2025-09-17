using System.Collections.Generic;

namespace FSH.Starter.Blazor.Client.Models.NavigationMenu;

public class MenuSectionItemModel
{
    public string Title { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? Href { get; set; }
    public string? Target { get; set; }
    public bool Disabled { get; set; }
    public bool IsParent { get; set; }
    public List<MenuSectionSubItemModel>? MenuItems { get; set; }
    public string[]? Roles { get; set; }
    public string? Action { get; set; }
    public string? Resource { get; set; }
    public PageStatus PageStatus { get; set; } = PageStatus.None;
}
