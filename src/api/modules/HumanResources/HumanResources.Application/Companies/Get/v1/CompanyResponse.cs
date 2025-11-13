namespace FSH.Starter.WebApi.HumanResources.Application.Companies.Get.v1;

/// <summary>
/// Response for company details.
/// </summary>
public sealed record CompanyResponse
{
    public DefaultIdType Id { get; init; }
    public string Code { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string? TIN { get; init; }
    public string? Address { get; init; }
    public string? ZipCode { get; init; }
    public string? Phone { get; init; }
    public string? Email { get; init; }
    public string? Website { get; init; }
    public string? LogoUrl { get; init; }
    public bool IsActive { get; init; }
}

