using MediatR;

namespace FSH.Starter.WebApi.App.Features.Update.v1;

public sealed record GroupUpdateCommand(
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
    string? Notes) : IRequest<GroupUpdateResponse>;
