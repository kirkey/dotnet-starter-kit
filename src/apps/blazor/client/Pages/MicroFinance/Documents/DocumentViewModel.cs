using FSH.Starter.Blazor.Infrastructure.Api;
using Mapster;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.Documents;

public class DocumentViewModel
{
    public DefaultIdType Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public string? MimeType { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public string? OriginalFileName { get; set; }
    public bool IsVerified { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public Guid? VerifiedById { get; set; }

    public class Mapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<DocumentResponse, DocumentViewModel>();
            config.NewConfig<DocumentViewModel, CreateDocumentCommand>();
        }
    }
}
