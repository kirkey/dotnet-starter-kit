using FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Get.v1;

public sealed class GetGeneratedDocumentHandler(
    [FromKeyedServices("hr:generateddocuments")] IReadRepository<GeneratedDocument> repository)
    : IRequestHandler<GetGeneratedDocumentRequest, GeneratedDocumentResponse>
{
    public async Task<GeneratedDocumentResponse> Handle(
        GetGeneratedDocumentRequest request,
        CancellationToken cancellationToken)
    {
        var document = await repository
            .FirstOrDefaultAsync(new GeneratedDocumentByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (document is null)
            throw new GeneratedDocumentNotFoundException(request.Id);

        return new GeneratedDocumentResponse
        {
            Id = document.Id,
            DocumentTemplateId = document.DocumentTemplateId,
            EntityId = document.EntityId,
            EntityType = document.EntityType,
            GeneratedContent = document.GeneratedContent,
            Status = document.Status,
            GeneratedDate = document.GeneratedDate,
            FinalizedDate = document.FinalizedDate,
            SignedDate = document.SignedDate,
            SignedBy = document.SignedBy,
            FilePath = document.FilePath,
            Version = document.Version,
            IsActive = document.IsActive,
            Notes = document.Notes
        };
    }
}

