namespace FSH.Starter.WebApi.HumanResources.Application.Companies.Specifications;

/// <summary>
/// Specification to get company by code.
/// </summary>
public class CompanyByCodeSpec : Specification<Company>
{
    public CompanyByCodeSpec(string code)
    {
        Query.Where(c => c.CompanyCode == code);
    }
}

