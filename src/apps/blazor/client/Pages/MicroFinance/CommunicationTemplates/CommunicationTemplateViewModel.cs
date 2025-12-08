namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CommunicationTemplates;

public class CommunicationTemplateViewModel
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Channel { get; set; }
    public string? Category { get; set; }
    public string? Body { get; set; }
    public string? Subject { get; set; }
    public string? Placeholders { get; set; }
    public string? Language { get; set; }
    public bool RequiresApproval { get; set; }
}
