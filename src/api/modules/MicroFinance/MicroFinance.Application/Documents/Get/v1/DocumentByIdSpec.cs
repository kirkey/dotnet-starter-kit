using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.Documents.Get.v1;

/// <summary>
/// Specification for getting a document by ID.
/// </summary>
public sealed class DocumentByIdSpec : Specification<Document, DocumentResponse>
{
    public DocumentByIdSpec(Guid id)
    {
        Query.Where(d => d.Id == id);

        Query.Select(d => new DocumentResponse(
            d.Id,
            d.Name,
            d.DocumentType,
            d.Category,
            d.Status,
            d.EntityType,
            d.EntityId,
            d.FilePath,
            d.MimeType,
            d.FileSizeBytes,
            d.Description,
            d.OriginalFileName,
            d.IssueDate,
            d.ExpiryDate,
            d.IssuingAuthority,
            d.DocumentNumber,
            d.IsVerified,
            d.VerifiedAt,
            d.IsRequired));
    }
}
