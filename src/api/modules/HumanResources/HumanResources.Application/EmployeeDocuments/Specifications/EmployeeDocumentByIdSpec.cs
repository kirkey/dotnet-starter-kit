namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Specifications;

public class EmployeeDocumentByIdSpec : Specification<EmployeeDocument>
{
    public EmployeeDocumentByIdSpec(DefaultIdType id)
    {
        Query.Where(d => d.Id == id);
    }
}

