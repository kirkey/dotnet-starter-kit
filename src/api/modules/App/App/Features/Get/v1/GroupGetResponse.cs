namespace FSH.Starter.WebApi.App.Features.Get.v1;

public record GroupGetResponse(
    DefaultIdType Id,
    string Application,
    string Parent,
    string? Tag,
    int Number,
    string Code,
    string Name,
    decimal Amount,
    DefaultIdType? EmployeeId,
    string? EmployeeName,
    string? Description,
    string? Notes);
