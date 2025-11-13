namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when a company is not found.
/// </summary>
public class CompanyNotFoundException : NotFoundException
{
    public CompanyNotFoundException(DefaultIdType id)
        : base($"Company with ID '{id}' was not found.")
    {
    }

    public CompanyNotFoundException(string companyCode)
        : base($"Company with code '{companyCode}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when a company code already exists.
/// </summary>
public class CompanyCodeAlreadyExistsException : ConflictException
{
    public CompanyCodeAlreadyExistsException(string companyCode)
        : base($"Company with code '{companyCode}' already exists.")
    {
    }
}
