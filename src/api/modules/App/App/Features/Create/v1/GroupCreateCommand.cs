using MediatR;

namespace FSH.Starter.WebApi.App.Features.Create.v1;

public record GroupCreateCommand(
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
    string? Notes)
    : IRequest<GroupCreateResponse>;
