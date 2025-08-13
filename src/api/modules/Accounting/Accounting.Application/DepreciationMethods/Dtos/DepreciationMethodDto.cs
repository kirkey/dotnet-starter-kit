using FSH.Framework.Core.Extensions.Dto;

namespace Accounting.Application.DepreciationMethods.Dtos;

public class DepreciationMethodDto(
    string methodCode,
    string? formula,
    bool isActive) : BaseDto
{
    public string MethodCode { get; set; } = methodCode;
    public string? Formula { get; set; } = formula;
    public bool IsActive { get; set; } = isActive;
}
