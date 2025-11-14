namespace FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Specifications;

public class GeneratedDocumentByIdSpec : Specification<GeneratedDocument>
{
    public GeneratedDocumentByIdSpec(DefaultIdType id)
    {
        Query.Where(d => d.Id == id);
    }
}

