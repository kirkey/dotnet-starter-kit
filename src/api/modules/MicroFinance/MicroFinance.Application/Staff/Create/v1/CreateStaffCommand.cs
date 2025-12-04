using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Staff.Create.v1;

public sealed record CreateStaffCommand(
    string EmployeeNumber,
    string FirstName,
    string LastName,
    string Email,
    string JobTitle,
    string Role,
    DateOnly JoiningDate,
    string EmploymentType = "FullTime",
    Guid? BranchId = null,
    string? Department = null,
    Guid? UserId = null) : IRequest<CreateStaffResponse>;
