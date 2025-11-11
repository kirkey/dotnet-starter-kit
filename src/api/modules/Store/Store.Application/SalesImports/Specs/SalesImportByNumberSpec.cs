namespace FSH.Starter.WebApi.Store.Application.SalesImports.Specs;

/// <summary>
/// Specification to find a sales import by import number.
/// </summary>
public class SalesImportByNumberSpec : Specification<SalesImport>
{
    public SalesImportByNumberSpec(string importNumber)
    {
        Query.Where(x => x.ImportNumber == importNumber);
    }
}

