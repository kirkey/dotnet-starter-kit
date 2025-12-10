namespace FSH.Starter.WebApi.MicroFinance.Application.Staff.Create.v1;

public sealed record CreateStaffResponse(
    DefaultIdType Id,
    string EmployeeNumber,
    string FullName,
    string Role,
    string Status);
