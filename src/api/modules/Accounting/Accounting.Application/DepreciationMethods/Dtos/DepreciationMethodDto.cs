namespace Accounting.Application.DepreciationMethods.Dtos;

public class DepreciationMethodDto
{
    public DefaultIdType Id { get; set; }
    public string MethodCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Formula { get; set; }
    public bool IsActive { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }

    public DepreciationMethodDto(
        DefaultIdType id,
        string methodCode,
        string name,
        string? formula,
        bool isActive,
        string? description,
        string? notes)
    {
        Id = id;
        MethodCode = methodCode;
        Name = name;
        Formula = formula;
        IsActive = isActive;
        Description = description;
        Notes = notes;
    }
}
