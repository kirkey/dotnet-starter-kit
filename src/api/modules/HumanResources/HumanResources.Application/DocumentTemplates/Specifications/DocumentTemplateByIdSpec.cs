namespace FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Specifications;

public class DocumentTemplateByIdSpec : Specification<DocumentTemplate>
{
    public DocumentTemplateByIdSpec(DefaultIdType id)
    {
        Query.Where(d => d.Id == id);
    }
}

