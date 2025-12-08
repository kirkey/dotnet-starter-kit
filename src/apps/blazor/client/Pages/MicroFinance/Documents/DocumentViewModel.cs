namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.Documents;

public class DocumentViewModel
{
    public string? Name { get; set; }
    public string? DocumentType { get; set; }
    public string? EntityType { get; set; }
    public Guid EntityId { get; set; }
    public string? FilePath { get; set; }
    public long FileSizeBytes { get; set; }
    public string? MimeType { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public string? OriginalFileName { get; set; }
}
