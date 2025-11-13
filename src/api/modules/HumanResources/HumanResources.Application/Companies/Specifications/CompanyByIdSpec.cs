namespace FSH.Starter.WebApi.HumanResources.Application.Companies.Specifications;

/// <summary>
/// Specification to get company by ID.
/// </summary>
public class CompanyByIdSpec : Specification<Company>
{
    public CompanyByIdSpec(DefaultIdType id)
    {
        Query.Where(c => c.Id == id);
    }
}

